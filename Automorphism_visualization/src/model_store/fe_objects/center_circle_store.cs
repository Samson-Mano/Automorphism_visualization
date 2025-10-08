using _2DHelmholtz_solver.global_variables;
using _2DHelmholtz_solver.src.model_store.geom_objects;
using _2DHelmholtz_solver.src.opentk_control.opentk_bgdraw;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automorphism_visualization.src.model_store.fe_objects
{
    public class center_circle_store
    {
        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        public meshdata_store center_circle;

        private int pt_count = 30;
        private bool isCircleDrag = false;

        // Drag influence controls the drrag and
        // boundary edge factor limits the boundary zone of unit circle 
        const float drag_influence_factor = 6.0f;
        const float boundary_edge_factor = 6.0f;

        private double unitcircleradius = 1000.0;

        private latitude_circle_store latitude_circles;
        private longitude_line_store longitude_lines;

        private verticalgrid_line_store verticalgrid_lines;
        private horizontalgrid_line_store horizontalgrid_lines;

        public center_circle_store()
        {

            // initialize the mesh data store
            center_circle = new meshdata_store();

            //__________________________________________________________________________
            // Create the center circle
            // Add the boundary points for center Circle

            for (int i = 0; i < pt_count; i++)
            {
                // Create the points for circle
                double angle = (2.0 * Math.PI * i) / (double)pt_count;
                double x = centerpt.X + (centerradius * Math.Cos(angle));
                double y = centerpt.Y + (centerradius * Math.Sin(angle));

                center_circle.add_mesh_point(i, x, y, 0.0, -1);

                if (i < pt_count - 1)
                {
                    center_circle.add_mesh_lines(i, i, i + 1, -2);

                }

            }

            center_circle.add_mesh_lines(pt_count - 1, pt_count - 1, 0, -2);
             // Create the shaders and buffers

            center_circle.set_shader();
            center_circle.set_buffer();

            // Set the latitude circles
            latitude_circles = new latitude_circle_store(centerradius);

            // Set the longitude lines
            longitude_lines = new longitude_line_store(centerradius);

            // Set the vertical grid lines
            verticalgrid_lines = new verticalgrid_line_store(centerradius);

            // Set the horizontal grid lines
            horizontalgrid_lines = new horizontalgrid_line_store(centerradius);

        }




        public void paint_center_circle()
        {
            // Paint the center circle
            gvariables_static.LineWidth = 2.0f;
            center_circle.paint_dynamic_mesh_lines();

            if(gvariables_static.is_polar_grid == true)
            {
                // Paint the latitude circles
                latitude_circles.paint_latitude_circles();

                // Paint the longitude lines
                longitude_lines.paint_longitude_lines();

            }
            else
            {
                // Paint the vertical grid lines
                verticalgrid_lines.paint_vertical_grid_lines();

                // Paint the horizontal grid lines
                horizontalgrid_lines.paint_horizontal_grid_lines();

            }

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of center circle
            center_circle.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency, 
                graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);


            latitude_circles.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                 graphic_events_control);

            longitude_lines.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                graphic_events_control);

            verticalgrid_lines.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                graphic_events_control);

            horizontalgrid_lines.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                graphic_events_control);


        }


        public void circle_drag_start(Vector2 o_pt, drawing_events graphic_events_control)
        {
            // Convert the center point to the screen point
            Vector4 sc_center_pt = graphic_events_control.projectionMatrix * graphic_events_control.viewMatrix
                                     * graphic_events_control.modelMatrix * new Vector4(centerpt.X, centerpt.Y, 0.0f, 1.0f);


            // Get the model and zoom scale
            double model_scale = graphic_events_control.modelMatrix[0, 0];
            double zoom_scale = graphic_events_control.viewMatrix[0, 0];

            // Get the 2D translation due to pan operation
            double x_transl = graphic_events_control.viewMatrix[0, 3];
            double y_transl = graphic_events_control.viewMatrix[1, 3];

            // Convert to screen raidus
            double sc_radius = model_scale * zoom_scale * (centerradius * drag_influence_factor);

            // Test whether the drag point start is within the circle 
            if (Vector2.Distance(o_pt, new Vector2(sc_center_pt.X, sc_center_pt.Y)) < sc_radius)
            {
                isCircleDrag = true;

                // Conver the screen point to model point
                Vector2 model_pt = new Vector2((float)((o_pt.X - x_transl) / (model_scale * zoom_scale)),
                    (float)((o_pt.Y - y_transl) / (model_scale * zoom_scale)));

                update_circle_location(model_pt);

            }

        }


        public void circle_drag_inprogress(Vector2 c_pt, drawing_events graphic_events_control)
        {
            // Drag is progress
            if (isCircleDrag == true)
            {

                // Get the model and zoom scale
                double model_scale = graphic_events_control.modelMatrix[0, 0];
                double zoom_scale = graphic_events_control.viewMatrix[0, 0];

                // Get the 2D translation due to pan operation
                double x_transl = graphic_events_control.viewMatrix[0, 3];
                double y_transl = graphic_events_control.viewMatrix[1, 3];

                // Conver the screen point to model point
                Vector2 model_pt = new Vector2((float)((c_pt.X - x_transl) / (model_scale * zoom_scale)),
                    (float)((c_pt.Y - y_transl) / (model_scale * zoom_scale)));

                update_circle_location(model_pt);

            }

        }


        public void circle_drag_end()
        {
            // Set drag end
            isCircleDrag = false;

        }


        public void update_circle_location(Vector2 new_CenterPt)
        {
            // Update circle location with new Center Point
            // MessageBox.Show($"Center pt X: {new_CenterPt.X}, Y: {new_CenterPt.Y}");

            // Find the length from origin
            double length_from_origin = Math.Sqrt(Math.Pow(new_CenterPt.X, 2) + Math.Pow(new_CenterPt.Y, 2));

            // test whether the center circle overlaps the unit circle boundary (or the origin)
            if (Math.Abs(length_from_origin - unitcircleradius) < (centerradius * boundary_edge_factor))
            {
                if (length_from_origin > unitcircleradius)
                {
                    // Outside the boundary of unit circle (still touching)
                    double c_ratio = (unitcircleradius + (centerradius * boundary_edge_factor)) / length_from_origin;
                    new_CenterPt = new Vector2((float)(c_ratio * new_CenterPt.X), (float)(c_ratio * new_CenterPt.Y));

                }
                else if (length_from_origin < unitcircleradius)
                {
                    // Inside the boundary of unit circle (still touching)
                    double c_ratio = (unitcircleradius - (centerradius * boundary_edge_factor)) / length_from_origin;
                    new_CenterPt = new Vector2((float)(c_ratio * new_CenterPt.X), (float)(c_ratio * new_CenterPt.Y));

                }
            }
            else if (length_from_origin < centerradius)
            {
                // Close to the origin zero
                new_CenterPt = new Vector2(0.0f, 0.0f);

            }

            // Update the center point
            this.centerpt = new_CenterPt;


            for (int i = 0; i < pt_count; i++)
            {
                // Create the points for circle
                double angle = (2.0 * Math.PI * i) / (double)pt_count;
                double x = centerpt.X + (centerradius * Math.Cos(angle));
                double y = centerpt.Y + (centerradius * Math.Sin(angle));

                center_circle.update_mesh_point(i, x, y, 0.0, -1);

            }

            if(gvariables_static.is_polar_grid == true)
            {
                // Update the center circle location to the latitude & longitude
                latitude_circles.update_centerpt(centerpt);

                longitude_lines.update_centerpt(centerpt);

            }
            else
            {
                // Update the center circle location to the vertical & horizontal grid lines
                verticalgrid_lines.update_centerpt(centerpt);

                horizontalgrid_lines.update_centerpt(centerpt);

            }



        }



    }
}
