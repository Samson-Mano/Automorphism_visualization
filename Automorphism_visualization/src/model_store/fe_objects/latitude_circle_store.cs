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
                //double angle, x, y;
                int pt_index1, pt_index2;

                List<Vector2> ClippedCirclePts = GetClippedCircle(t_origin, t_circle_radius, segment_count);

                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for circle segments
                    // First point 
                    pt_index1 = (2 * j) + 0;
                    //angle = (2.0 * Math.PI * j) / (double)segment_count;
                    //x = t_origin.X + (t_circle_radius * Math.Cos(angle));
                    //y = t_origin.Y + (t_circle_radius * Math.Sin(angle));

                    //Vector2 clipped_pt = GetClippedCirclePts(new Vector2((float)x, (float)y));

                    latitude_circles[i].update_mesh_point(pt_index1, ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    //angle = (2.0 * Math.PI * (j + 1)) / (double)segment_count;
                    //x = t_origin.X + (t_circle_radius * Math.Cos(angle));
                    //y = t_origin.Y + (t_circle_radius * Math.Sin(angle));

                    //clipped_pt = GetClippedCirclePts(new Vector2((float)x, (float)y));

                    latitude_circles[i].update_mesh_point(pt_index2, ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, -1);

                }

            }


        }


        private List<Vector2> GetClippedCircle(Vector2 center, double radius, int segment_count)
        {

            // Step 1: Find intersection points between circle and rectangle edges
            List<(double angle, Vector2 pt)> intersections = getIntersectionsofCircleandRectangle(center, radius);
            List<Vector2> ClippedCirclePts = new List<Vector2>();

            double angle, x, y;
            int pt_index1, pt_index2;

            if (intersections.Count == 0)
            {
                // Either the circle is inside the rectangle or outside the rectangle 
                // Check whether the circle is outside the rectangle

                if (IsPointInsideRectangle(new Vector2((float)(center.X + radius), center.Y)) == true)
                {
                    // Circle is entirely inside the rectangle (So Use all the points)
                    for (int j = 0; j < segment_count; j++)
                    {
                        // Create the points for circle segments
                        // First point 
                        pt_index1 = (2 * j) + 0;
                        angle = (2.0 * Math.PI * j) / (double)segment_count;
                        x = center.X + (radius * Math.Cos(angle));
                        y = center.Y + (radius * Math.Sin(angle));

                        ClippedCirclePts.Add(new Vector2((float)x, (float)y));

                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        angle = (2.0 * Math.PI * (j + 1)) / (double)segment_count;
                        x = center.X + (radius * Math.Cos(angle));
                        y = center.Y + (radius * Math.Sin(angle));

                        ClippedCirclePts.Add(new Vector2((float)x, (float)y));

                    }

                }
                else
                {
                    // Circle is entirely outside the rectangle (So null points)
                    for (int j = 0; j < segment_count; j++)
                    {
                        // Create the points for circle segments
                        // First Null point 
                        pt_index1 = (2 * j) + 0;
                        ClippedCirclePts.Add(new Vector2(-4000, -4000));

                        // Second Null point 
                        pt_index2 = (2 * j) + 1;
                        ClippedCirclePts.Add(new Vector2(-4000, -4000));

                    }

                }

                // Exit the function here
                return ClippedCirclePts;

            }


            // Step 2: Sort intersections by angle around circle
            intersections.Sort((a, b) => a.angle.CompareTo(b.angle));

            // Step 3: Walk around circle in angular order
            // For each arc between consecutive intersections, test midpoint
            // If midpoint is inside rectangle, keep the arc
            // Then discretize arc into N points (proportional to arc length)

            List<(int pair_index, double arc_length)> arc_data = new List<(int pair_index, double arc_length)>();
            double total_arc_length = 0.0;

            for (int i = 0; i < intersections.Count; i += 2)
            {
                // Pair up consecutive intersections
                var start = intersections[i];
                var end = intersections[(i + 1) % intersections.Count]; // wrap around

                // Compute midpoint angle
                double midAngle = (start.angle + end.angle) / 2.0d;

                if (end.angle < start.angle)
                    midAngle += Math.PI; // optional wrap-around correction

                // Convert midpoint angle to point on circle
                Vector2 midPt = new Vector2(
                    center.X + (float)(radius * Math.Cos(midAngle)),
                    center.Y + (float)(radius * Math.Sin(midAngle))
                );

                //  Check if midpoint is inside rectangle
                if (IsPointInsideRectangle(midPt) == true)
                {

                    // Compute arc length
                    double angleSpan = end.angle - start.angle;
                    if (angleSpan < 0) angleSpan += 2 * Math.PI;


                    //  Find the arc length
                    double arc_length = radius * angleSpan;

                    // Sum to the total arc length
                    total_arc_length = total_arc_length + arc_length;

                    // Add to the arc data
                    arc_data.Add((i, arc_length));
                }

            }


            // Step 3B: Once again null intersection case
            if (arc_data.Count == 0)
            {
                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for circle segments
                    // First Null point 
                    pt_index1 = (2 * j) + 0;
                    ClippedCirclePts.Add(new Vector2(-4000, -4000));

                    // Second Null point 
                    pt_index2 = (2 * j) + 1;
                    ClippedCirclePts.Add(new Vector2(-4000, -4000));

                }

                return ClippedCirclePts;
            }




            // Step 4: Find the points to connect between arcs
            // Step 4A: Find the segment count for each arc segments to make sure the total count = segment_count

            var arcSegments = new int[arc_data.Count];
            var remainders = new List<(int index, double remainder)>();

            int allocatedSegments = 0;

            // Step 4A Step 1: Initial allocation
            for (int i = 0; i < arc_data.Count; i++)
            {
                double rawCount = (arc_data[i].arc_length / total_arc_length) * segment_count;
                int count = (int)Math.Floor(rawCount);
                arcSegments[i] = count;
                allocatedSegments += count;

                double remainder = rawCount - count;
                remainders.Add((i, remainder));
            }

            // Step 4A Step 2: Distribute remaining segments
            int remaining = segment_count - allocatedSegments;
            remainders.Sort((a, b) => b.remainder.CompareTo(a.remainder)); // descending

            for (int i = 0; i < remaining; i++)
            {
                arcSegments[remainders[i].index]++;
            }


            int m = 0;
            foreach ((int pair_index, double arc_length) a_data in arc_data)
            {
                // Get the consecutive intersections which forms the arc inside the circle
                var start = intersections[a_data.pair_index];
                var end = intersections[(a_data.pair_index + 1) % intersections.Count]; // wrap around

                double angleSpan = end.angle - start.angle;
                if (angleSpan < 0) angleSpan += 2 * Math.PI;


                for (int j = 0; j < arcSegments[m]; j++)
                {

                    // Point 1 of the segment
                    pt_index1 = (2 * j) + 0;
                    double t = (double)j / (double)arcSegments[m];
                    angle = start.angle + (t * angleSpan);
                    Vector2 pt1 = new Vector2(
                        center.X + (float)(radius * Math.Cos(angle)),
                        center.Y + (float)(radius * Math.Sin(angle))
                    );
                    ClippedCirclePts.Add(pt1);

                    // Point 2 of the segment
                    pt_index2 = (2 * j) + 1;
                    t = (double)(j + 1) / (double)arcSegments[m];
                    angle = start.angle + (t * angleSpan);
                    Vector2 pt2 = new Vector2(
                        center.X + (float)(radius * Math.Cos(angle)),
                        center.Y + (float)(radius * Math.Sin(angle))
                    );
                    ClippedCirclePts.Add(pt2);

                }

                m++;
            }

            if(ClippedCirclePts.Count < segment_count * 2 )
            {
                int stop = -1;

            }


            return ClippedCirclePts; // build the final clipped polygon

        }



        private List<(double angle, Vector2 pt)> getIntersectionsofCircleandRectangle(Vector2 center, double radius)
        {
            // Rectangle boundaries
            const float xmin = -4000, xmax = 4000, ymin = -4000, ymax = 4000;

            List<(double angle, Vector2 pt)> intersections = new List<(double angle, Vector2 pt)>();

            // Check each edge of rectangle: (x = const, or y = const)
            // Solve circle equation (x - cx)^2 + (y - cy)^2 = r^2
            // Keep only those within segment bounds
            // Store intersection and its polar angle relative to circle center

            //  A circle centered at center = (cx, cy) with radius r.
            //A rectangle defined by vertical edges at x = xmin, x = xmax and horizontal edges at y = ymin, y = ymax.

            //The circle equation is:
            //(x−cx)2 + (y−cy)2 = r2(x - cx) ^ 2 + (y - cy) ^ 2 = r ^ 2(x−cx)2 + (y−cy)2 = r2
            //We’ll intersect this with each of the 4 rectangle edges:

            //1.Vertical edges(x = xmin and x = xmax)
            //Plug into the circle equation:
            //(xmin - cx) ^ 2 + (y - cy) ^ 2 = r ^ 2
            //Solve for y:
            //y = cy +/- sqrt(r^2 − (xmin − cx)^2)
            //
            //Only keep y values within[ymin, ymax].
            //Repeat for xmax.


            // Vertical edges
            foreach (float x in new[] { xmin, xmax })
            {
                double dx = x - center.X;
                double discriminant = (radius * radius) - (dx * dx);
                if (discriminant >= 0)
                {
                    double sqrt = Math.Sqrt(discriminant);
                    foreach (double y in new[] { center.Y + sqrt, center.Y - sqrt })
                    {
                        if (y >= ymin && y <= ymax)
                        {
                            var pt = new Vector2(x, (float)y);
                            double angle = Math.Atan2(y - center.Y, x - center.X);
                            intersections.Add((angle, pt));
                        }
                    }
                }
            }

            //2.Horizontal edges(y = ymin and y = ymax)
            //Plug into the circle equation:
            //(x - cx) ^ 2 + (ymin - cy) ^ 2 = r ^ 2
            //Solve for x:
            //x = cx +/- sqrt(r^2 - (ymin - cy) ^ 2)
            //
            //Only keep x values within[xmin, xmax].
            //Repeat for ymax.

            // Horizontal edges
            foreach (float y in new[] { ymin, ymax })
            {
                double dy = y - center.Y;
                double discriminant = (radius * radius) - (dy * dy);
                if (discriminant >= 0)
                {
                    double sqrt = Math.Sqrt(discriminant);
                    foreach (double x in new[] { center.X + sqrt, center.X - sqrt })
                    {
                        if (x >= xmin && x <= xmax)
                        {
                            var pt = new Vector2((float)x, y);
                            double angle = Math.Atan2(y - center.Y, x - center.X);
                            intersections.Add((angle, pt));
                        }
                    }
                }
            }

            return intersections;

        }

        private bool IsPointInsideRectangle(Vector2 pt)
        {
            // Rectangle boundaries
            const float xmin = -4000, xmax = 4000, ymin = -4000, ymax = 4000;
            return pt.X > xmin && pt.X < xmax && pt.Y > ymin && pt.Y < ymax;

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
