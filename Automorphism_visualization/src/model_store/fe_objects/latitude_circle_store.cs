using _2DHelmholtz_solver.global_variables;
using _2DHelmholtz_solver.src.model_store.geom_objects;
using _2DHelmholtz_solver.src.opentk_control.opentk_bgdraw;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automorphism_visualization.src.model_store.fe_objects
{
    public class latitude_circle_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        private int num_circles = 10;

        public List<meshdata_store> latitude_circles = new List<meshdata_store>();

        private int segment_count = 100;

        private double unitcircleradius = 1000.0;




        public latitude_circle_store(double centerradius)
        {
            // center radius
            this.centerradius = centerradius;

            // Lattidue circles radius
            double radius_interval = ((unitcircleradius - centerradius) / (double)(num_circles + 1));

            for (int i = 0; i < num_circles; i++)
            {
                // Get the circle radius
                double temp_latitude_circle_radius = centerradius + ((i + 1) * radius_interval);

                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_latitude_circle = new meshdata_store();

                //__________________________________________________________________________
                // Create the Latitude circle
                // Add the boundary points for Latidue Circle
                double angle, x, y;
                int pt_index1, pt_index2;

                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for circle segments
                    // First point 
                    pt_index1 = (2 * j) + 0;
                    angle = (2.0 * Math.PI * j) / (double)segment_count;
                    x = centerpt.X + (temp_latitude_circle_radius * Math.Cos(angle));
                    y = centerpt.Y + (temp_latitude_circle_radius * Math.Sin(angle));

                    temp_latitude_circle.add_mesh_point(pt_index1, x, y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    angle = (2.0 * Math.PI * (j + 1)) / (double)segment_count;
                    x = centerpt.X + (temp_latitude_circle_radius * Math.Cos(angle));
                    y = centerpt.Y + (temp_latitude_circle_radius * Math.Sin(angle));

                    temp_latitude_circle.add_mesh_point(pt_index2, x, y, 0.0, -1);

                    // Set the line index
                    temp_latitude_circle.add_mesh_lines(j, pt_index1, pt_index2, 2);

                }

                // Create the shaders and buffers
                temp_latitude_circle.set_shader();
                temp_latitude_circle.set_buffer();

                // Add to the latitude circle list
                latitude_circles.Add(temp_latitude_circle);

            }

        }


        public void paint_latitude_circles()
        {
            // Paint the latitude circles
            gvariables_static.LineWidth = 2.0f;

            for (int i = 0; i < latitude_circles.Count; i++)
            {
                latitude_circles[i].paint_dynamic_mesh_lines();

            }
            

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of latitude circles

            for (int i = 0; i < latitude_circles.Count; i++)
            {
                latitude_circles[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency, 
                    graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);


            }

        }


        public void update_centerpt(Vector2 new_CenterPt)
        {
  
            // Lattidue circles radius
            double radius_interval = ((unitcircleradius - centerradius) / (double)(num_circles + 1));

            for (int i = 0; i < num_circles; i++)
            {
                // Get the circle radius before transformation
                Vector2 t_origin = new Vector2(0.0f, 0.0f);
                double t_circle_radius = centerradius + ((i + 1) * radius_interval);


                if (new_CenterPt != new Vector2(0, 0))
                {
                    // Find the intersection points of circle and the new_CenterPt vector
                    double newCenterPt_amplitude = Vector2.Distance(new_CenterPt, new Vector2(0, 0));

                    double temp_latitude_circle_radius_ratio = t_circle_radius / newCenterPt_amplitude;

                    Vector2 circle_closest_point = Vector2.Lerp(new Vector2(0, 0), new_CenterPt, (float)temp_latitude_circle_radius_ratio);
                    Vector2 circle_other_point = Vector2.Lerp(new Vector2(0, 0), new_CenterPt, -(float)temp_latitude_circle_radius_ratio);

                    // Transformed circle closest and farther point
                    Vector2 trans_circle_other_pt = transformation_function(circle_other_point / 1000.0f, new_CenterPt / 1000.0f);
                    Vector2 trans_circle_closest_pt = transformation_function(circle_closest_point / 1000.0f, new_CenterPt / 1000.0f);

                    t_origin = (trans_circle_other_pt + trans_circle_closest_pt) * 0.5f * 1000.0f;

                    // Transformed circle radius
                    t_circle_radius = Vector2.Distance(trans_circle_other_pt, trans_circle_closest_pt) * 0.5 * 1000.0;
                }


                //__________________________________________________________________________
                // Create the Latitude circle
                // Update the boundary points for Latidue Circle
                double angle, x, y;
                int pt_index1, pt_index2;


                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for circle segments
                    // First point 
                    pt_index1 = (2 * j) + 0;
                    angle = (2.0 * Math.PI * j) / (double)segment_count;
                    x = t_origin.X + (t_circle_radius * Math.Cos(angle));
                    y = t_origin.Y + (t_circle_radius * Math.Sin(angle));

                    Vector2 clipped_pt = GetClippedCirclePts(new Vector2((float)x, (float)y));

                    latitude_circles[i].update_mesh_point(pt_index1, clipped_pt.X, clipped_pt.Y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    angle = (2.0 * Math.PI * (j + 1)) / (double)segment_count;
                    x = t_origin.X + (t_circle_radius * Math.Cos(angle));
                    y = t_origin.Y + (t_circle_radius * Math.Sin(angle));

                   clipped_pt = GetClippedCirclePts(new Vector2((float)x, (float)y));

                    latitude_circles[i].update_mesh_point(pt_index2, clipped_pt.X, clipped_pt.Y, 0.0, -1);

                }

            }


        }


        private Vector2[] get_circle_clipped_with_rectangle(Vector2 circle_centerpt, double circle_radius)
        {

            // Clipping rectangle
            Vector2 clip_rectangle_pt1 = new Vector2(-4000, -4000);
            Vector2 clip_rectangle_pt2 = new Vector2(-4000, 4000);
            Vector2 clip_rectangle_pt3 = new Vector2(4000, 4000);
            Vector2 clip_rectangle_pt4 = new Vector2(4000, -4000);

            // Points on the circle clipped by rectangle
            int circle_pt_count = 100;
            Vector2[] circle_pts = new Vector2[circle_pt_count];

            for (int j = 0; j < circle_pt_count; j++)
            {
                // Create the points for circle

                double angle = (2.0 * Math.PI * j) / (double)circle_pt_count;
                double cpt_x = circle_centerpt.X + (circle_radius * Math.Cos(angle));
                double cpt_y = circle_centerpt.Y + (circle_radius * Math.Sin(angle));


                // Add to the circle pts
                circle_pts[j] = new Vector2((float)cpt_x, (float)cpt_y);

            }

            return circle_pts;

        }


        private Vector2 GetClippedCirclePts(Vector2 cpt)
        {
            // Rectangle boundaries
            float xmin = -4000;
            float xmax = 4000;
            float ymin = -4000;
            float ymax = 4000;


            // Clip against rectangle
            float clippedX = Math.Min(Math.Max(cpt.X, xmin), xmax);
            float clippedY = Math.Min(Math.Max(cpt.Y, ymin), ymax);

            return new Vector2(clippedX, clippedY);

        }




        private Vector2 transformation_function(Vector2 Zval, Vector2 Cval)
        {
            // Numerator
            double a = Zval.X + Cval.X;
            double b = Zval.Y + Cval.Y;

            // Denominator
            double u = 1 + (Cval.X * Zval.X) + (Cval.Y * Zval.Y);
            double v = (Cval.X * Zval.Y) - (Cval.Y * Zval.X);
            double denom = (u * u) + (v * v);

            // Final transformed coordinates
            double fz_x = ((a * u) + (b * v)) / denom;
            double fz_y = ((b * u) - (a * v)) / denom;



            //// Numerator
            //double a = z_x + c_x;
            //double b = z_y + c_y;

            //// Denominator
            //double u = 1 + (c_x * z_x) + (c_y * z_y);
            //double v = (c_x * z_y) - (c_y * z_x);
            //double denom = (u * u) + (v * v);

            //// Final transformed coordinates
            //double fz_x = ((a * u) + (b * v)) / denom;
            //double fz_y = ((b * u) - (a * v)) / denom;




            return new Vector2((float)fz_x, (float)fz_y);

        }


    }
}
