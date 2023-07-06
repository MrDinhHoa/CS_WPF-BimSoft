using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Collections;
using Autodesk.AutoCAD.EditorInput;

namespace Lesson09A_AcadNet_ViewPort
{
    public static class Extensions
    {
        /// <summary>
        /// Reverses the order of the X and Y properties of a Point2d.
        /// </summary>
        /// <param name="flip">Boolean indicating whether to reverse or not.</param>
        /// <returns>The original Point2d or the reversed version.</returns>
        public static Point2d Swap(this Point2d pt, bool flip = true)
        { return flip ? new Point2d(pt.Y, pt.X) : pt;}

        /// <summary>
        /// Pads a Point2d with a zero Z value, returning a Point3d.
        /// </summary>
        /// <param name="pt">The Point2d to pad.</param>
        /// <returns>The padded Point3d.</returns>
        public static Point3d Pad(this Point2d pt)
        { return new Point3d(pt.X, pt.Y, 0);}

        /// <summary>
        /// Strips a Point3d down to a Point2d by simply ignoring the Z ordinate.
        /// </summary>
        /// <param name="pt">The Point3d to strip.</param>
        /// <returns>The stripped Point2d.</returns>
        public static Point2d Strip(this Point3d pt)
        {return new Point2d(pt.X, pt.Y);}

        /// <summary>
        /// Creates a layout with the specified name and optionally makes it current.
        /// </summary>
        /// <param name="name">The name of the viewport.</param>
        /// <param name="select">Whether to select it.</param>
        /// <returns>The ObjectId of the newly created viewport.</returns>
        public static ObjectId CreateAndMakeLayoutCurrent(this LayoutManager lm, string name, bool select = true)
        {
            // First try to get the layout
            var id = lm.GetLayoutId(name);
            // If it doesn't exist, we create it
            if (!id.IsValid)
            {id = lm.CreateLayout(name);}
            // And finally we select it
            if (select)
            {lm.CurrentLayout = name;}
            return id;
        }

        /// <summary>
        /// Applies an action to the specified viewport from this layout.
        /// Creates a new viewport if none is found withthat number.
        /// </summary>
        /// <param name="tr">The transaction to use to open the viewports.</param>
        /// <param name="vpNum">The number of the target viewport.</param>
        /// <param name="f">The action to apply to each of the viewports.</param>
        public static void ApplyToViewport(this Layout lay, Transaction tr, int vpNum, Action<Viewport> f)
        {
            ObjectIdCollection vpIds = lay.GetViewports();
            Viewport vp = null;
            foreach (ObjectId vpId in vpIds)
            {
                Viewport vp2 = tr.GetObject(vpId, OpenMode.ForWrite) as Viewport;
                if (vp2 != null && vp2.Number == vpNum)
                {
                    // We have found our viewport, so call the action
                    vp = vp2;
                    break;
                }
            }

            if (vp == null)
            {
                // We have not found our viewport, so create one
                BlockTableRecord btr =(BlockTableRecord)tr.GetObject(lay.BlockTableRecordId, OpenMode.ForWrite);
                vp = new Viewport();
                // Add it to the database
                btr.AppendEntity(vp);
                tr.AddNewlyCreatedDBObject(vp, true);
                // Turn it - and its grid - on
                vp.On = true;
                vp.GridOn = true;
            }
            // Finally we call our function on it
            f(vp);
        }

        /// <summary>
        /// Apply plot settings to the provided layout.
        /// </summary>
        /// <param name="pageSize">The canonical media name for our page size.</param>
        /// <param name="styleSheet">The pen settings file (ctb or stb).</param>
        /// <param name="devices">The name of the output device.</param>
        public static void SetPlotSettings(this Layout lay, string pageSize, string styleSheet, string device)
        {
            using (PlotSettings ps = new PlotSettings(lay.ModelType))
            {
               ps.CopyFrom(lay);
                PlotSettingsValidator psv = PlotSettingsValidator.Current;
                // Set the device
                System.Collections.Specialized.StringCollection devs = psv.GetPlotDeviceList();
                if (devs.Contains(device))
                {
                    psv.SetPlotConfigurationName(ps, device, null);
                    psv.RefreshLists(ps);
                }
                // Set the media name/size
                System.Collections.Specialized.StringCollection mns = psv.GetCanonicalMediaNameList(ps);
                if (mns.Contains(pageSize))
                {psv.SetCanonicalMediaName(ps, pageSize);}
                // Set the pen settings
                System.Collections.Specialized.StringCollection ssl = psv.GetPlotStyleSheetList();
                if (ssl.Contains(styleSheet))
                {psv.SetCurrentStyleSheet(ps, styleSheet);}
                // Copy the PlotSettings data back to the Layout
                bool upgraded = false;
                if (!lay.IsWriteEnabled)
                {
                    lay.UpgradeOpen();
                    upgraded = true;
                }
                lay.CopyFrom(ps);
                if (upgraded)
                { lay.DowngradeOpen();}
            }

        }

        /// <summary>
        /// Determine the maximum possible size for this layout.
        /// </summary>
        /// <returns>The maximum extents of the viewport on this layout.</returns>
        public static Extents2d GetMaximumExtents(this Layout lay)
        {
            // If the drawing template is imperial, we need to divide by
            // 1" in mm (25.4)
            //double div = lay.PlotPaperUnits == PlotPaperUnit.Inches ? 25.4 : 1.0;
            double div = lay.PlotPaperUnits == PlotPaperUnit.Millimeters ? 1.0 : 1.0;
            // We need to flip the axes if the plot is rotated by 90 or 270 deg
            bool doIt =lay.PlotRotation == PlotRotation.Degrees090 ||lay.PlotRotation == PlotRotation.Degrees270;
            // Get the extents in the correct units and orientation
            Point2d min = lay.PlotPaperMargins.MinPoint / div;
            Point2d max = (lay.PlotPaperSize -lay.PlotPaperMargins.MaxPoint.GetAsVector()) / div;
            return new Extents2d(min, max);
        }

        /// <summary>
        /// Sets the size of the viewport according to the provided extents.
        /// </summary>
        /// <param name="ext">The extents of the viewport on the page.</param>
        /// <param name="fac">Optional factor to provide padding.</param>
        public static void ResizeViewport(this Viewport vp, Extents2d ext, double fac = 1.0)
        {
            vp.Width = (ext.MaxPoint.X - ext.MinPoint.X) * fac;
            vp.Height = (ext.MaxPoint.Y - ext.MinPoint.Y) * fac;
            vp.CenterPoint = (Point2d.Origin + (ext.MaxPoint - ext.MinPoint) * 0.5).Pad();
        }

        /// <summary>
        /// Sets the view in a viewport to contain the specified model extents.
        /// </summary>
        /// <param name="ext">The extents of the content to fit the viewport.</param>
        /// <param name="fac">Optional factor to provide padding.</param>
        public static void FitContentToViewport(this Viewport vp, Extents3d ext, double fac = 1.0)
        {

            // Let's zoom to just larger than the extents
            vp.ViewCenter =(ext.MinPoint + ((ext.MaxPoint - ext.MinPoint) * 0.5)).Strip();
            // Get the dimensions of our view from the database extents
            double hgt = ext.MaxPoint.Y - ext.MinPoint.Y;
            double wid = ext.MaxPoint.X - ext.MinPoint.X;
            // We'll compare with the aspect ratio of the viewport itself
            // (which is derived from the page size)
            double aspect = vp.Width / vp.Height;
            // If our content is wider than the aspect ratio, make sure we
            // set the proposed height to be larger to accommodate the
            // content

            if (wid / hgt > aspect)
            { hgt = wid / aspect;}
            // Set the height so we're exactly at the extents
            vp.ViewHeight = hgt;
            // Set a custom scale to zoom out slightly (could also
            // vp.ViewHeight *= 1.1, for instance)
            vp.CustomScale = fac;
        }
    }

    public class Commands
    {
        [CommandMethod("CL")]
        public void CreateLayout()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
                return;
            Database db = doc.Database;
            Autodesk.AutoCAD.EditorInput.Editor ed = doc.Editor;
            Extents2d ext = new Extents2d();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Create and select a new layout tab
                ObjectId id = LayoutManager.Current.CreateAndMakeLayoutCurrent("NewLayout");
                // Open the created layout
                Layout lay = (Layout)tr.GetObject(id, OpenMode.ForWrite);
                // Make some settings on the layout and get its extents
                lay.SetPlotSettings
                    (
                        //"ISO_full_bleed_2A0_(1189.00_x_1682.00_MM)", // Try this big boy!
                        "ISO_full_bleed_A1_(841.00_x_594.00_MM)",
                        "monochrome.ctb",
                        "DWF6 ePlot.pc3"
                    );
                ext = lay.GetMaximumExtents();
                lay.ApplyToViewport
                (
                    tr, 2,
                    vp =>
                     {
                      // Size the viewport according to the extents calculated when
                      // we set the PlotSettings (device, page size, etc.)
                      // Use the standard 10% margin around the viewport
                      // (found by measuring pixels on screenshots of Layout1, etc.)
                      vp.ResizeViewport(ext, 1);
                      // Adjust the view so that the model contents fit
                      if (ValidDbExtents(db.Extmin, db.Extmax))
                        {vp.FitContentToViewport(new Extents3d(db.Extmin, db.Extmax), 1);}
                      // Finally we lock the view to prevent meddling
                      vp.Locked = true;
                     }
                );
                // Commit the transaction
                tr.Commit();
            }
            // Zoom so that we can see our new layout, again with a little padding
            ed.Command("_.ZOOM", "_E");
            ed.Command("_.ZOOM", ".7X");
            ed.Regen();
        }

        // Returns whether the provided DB extents - retrieved from
        // Database.Extmin/max - are "valid" or whether they are the default
        // invalid values (where the min's coordinates are positive and the
        // max coordinates are negative)
        private bool ValidDbExtents(Point3d min, Point3d max)
        {
            return
              !(min.X > 0 && min.Y > 0 && min.Z > 0 &&
                max.X < 0 && max.Y < 0 && max.Z < 0);

        }

        [CommandMethod("ViewportZoom")]
        static public void CommandChangeViewportZoom()

        {

            // access database and editor

            Database db = Application.DocumentManager.

              MdiActiveDocument.Database;

            Editor ed = Application.DocumentManager.

              MdiActiveDocument.Editor;



            using (Transaction tr = db.

              TransactionManager.StartTransaction())

            {

                LayoutManager layoutMgr = LayoutManager.Current;

                Layout layoutObj;

                DBDictionary layoutDict;



                ed.WriteMessage("Number of Layouts = {0}\n",

                  layoutMgr.LayoutCount);

                ed.WriteMessage("Current Layout = {0}\n",

                  layoutMgr.CurrentLayout);



                Point3d x_Min = db.Extmin;

                Point3d x_Max = db.Extmax;



                using (layoutDict = tr.GetObject(db.LayoutDictionaryId,

                  OpenMode.ForRead) as DBDictionary)

                {

                    foreach (DictionaryEntry layoutEntry in layoutDict)

                    {

                        using (layoutObj = tr.GetObject(

                          (ObjectId)(layoutEntry.Value),

                          OpenMode.ForRead) as Layout)

                        {



                            // Set the CurrentLayout to the layout

                            // if it is not Modelspace

                            if (layoutObj.LayoutName != "Model")

                                layoutMgr.CurrentLayout = layoutObj.LayoutName;



                            ed.WriteMessage("Layout Name = {0}\n",

                              layoutObj.LayoutName);

                            BlockTableRecord r = tr.GetObject(

                              layoutObj.BlockTableRecordId,

                              OpenMode.ForRead) as BlockTableRecord;

                            foreach (ObjectId obj in r)

                            {

                                DBObject dbobj = tr.GetObject(obj, OpenMode.ForRead);

                                Viewport vp = dbobj as Viewport;

                                if (vp != null)

                                {

                                    ed.WriteMessage("\nnumber of Viewport = {0}",

                                      vp.Number);



                                    // get the screen aspect ratio to calculate

                                    // the height and width

                                    double mScrRatio;

                                    // width/height

                                    mScrRatio = (vp.Width / vp.Height);



                                    Point3d mMaxExt = db.Extmax;

                                    Point3d mMinExt = db.Extmin;



                                    Extents3d mExtents = new Extents3d();

                                    mExtents.Set(mMinExt, mMaxExt);



                                    // prepare Matrix for DCS to WCS transformation

                                    Matrix3d matWCS2DCS;

                                    matWCS2DCS = Matrix3d.PlaneToWorld(vp.ViewDirection);

                                    matWCS2DCS = Matrix3d.Displacement(

                                      vp.ViewTarget - Point3d.Origin)

                                      * matWCS2DCS;

                                    matWCS2DCS = Matrix3d.Rotation(

                                      -vp.TwistAngle, vp.ViewDirection,

                                      vp.ViewTarget) * matWCS2DCS;

                                    matWCS2DCS = matWCS2DCS.Inverse();



                                    // tranform the extents to the DCS

                                    // defined by the viewdir

                                    mExtents.TransformBy(matWCS2DCS);



                                    // width of the extents in current view

                                    double mWidth;

                                    mWidth = (mExtents.MaxPoint.X - mExtents.MinPoint.X);



                                    // height of the extents in current view

                                    double mHeight;

                                    mHeight = (mExtents.MaxPoint.Y - mExtents.MinPoint.Y);



                                    // get the view center point

                                    Point2d mCentPt = new Point2d(

                                      ((mExtents.MaxPoint.X + mExtents.MinPoint.X) * 0.5),

                                      ((mExtents.MaxPoint.Y + mExtents.MinPoint.Y) * 0.5));



                                    // check if the width 'fits' in current window,

                                    // if not then get the new height as

                                    // per the viewports aspect ratio

                                    if (mWidth > (mHeight * mScrRatio))

                                        mHeight = mWidth / mScrRatio;



                                    // set the viewport parameters

                                    if (vp.Number == 2)

                                    {

                                        vp.UpgradeOpen();

                                        // set the view height - adjusted by 1%

                                        vp.ViewHeight = mHeight * 1.01;

                                        // set the view center

                                        vp.ViewCenter = mCentPt;

                                        vp.Visible = true;

                                        vp.On = true;

                                        vp.UpdateDisplay();

                                        ed.SwitchToModelSpace();

                                        Application.SetSystemVariable("CVPORT", vp.Number);

                                    }

                                    if (vp.Number == 3)

                                    {

                                        vp.UpgradeOpen();

                                        vp.ViewHeight = mHeight * 1.25;

                                        //set the view center

                                        vp.ViewCenter = mCentPt;

                                        vp.Visible = true;

                                        vp.On = true;

                                        vp.UpdateDisplay();

                                        ed.SwitchToModelSpace();

                                        Application.SetSystemVariable("CVPORT", vp.Number);

                                    }

                                }

                            }

                        }

                    }

                }

                tr.Commit();

            }

        }

    }

    
}
