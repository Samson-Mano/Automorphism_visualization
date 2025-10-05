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
        private int num_of_inside_circles = 10;
        private int num_of_outside_circles = 10;

        public List<meshdata_store> latitude_inside_circles = new List<meshdata_store>();
        public List<meshdata_store> latitude_outside_circles = new List<meshdata_store>();

        private int segment_count = 60;

        const double unitcircleradius = 1000.0;
        const float boundary_size = 4000.0f;



        public latitude_circle_store(double centerradius)
        {
            // center radius
            this.centerradius = centerradius;

            // Create latitude inside circle
            // Latitude inside circles radius interval
            double inside_circle_radius_interval = ((unitcircleradius - centerradius) / (double)(num_of_inside_circles + 1));

            for (int i = 0; i < num_of_inside_circles; i++)
            {
                // Get the circle radius
                double temp_latitude_circle_radius = centerradius + ((i + 1) * inside_circle_radius_interval);

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
                    temp_latitude_circle.add_mesh_lines(j, pt_index1, pt_index2, -3);

                }

                // Create the shaders and buffers
                temp_latitude_circle.set_shader();
                temp_latitude_circle.set_buffer();

                // Add to the latitude circle list
                latitude_inside_circles.Add(temp_latitude_circle);

            }


            // Create latitude outside circle
            // Latitude outside circles radius interval
            double outside_circle_radius_interval = ((boundary_size - unitcircleradius) / (double)(num_of_outside_circles + 1));

            for (int i = 0; i < num_of_outside_circles; i++)
            {
                // Get the circle radius
                double temp_latitude_circle_radius = unitcircleradius + ((i + 1) * outside_circle_radius_interval);

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
                    temp_latitude_circle.add_mesh_lines(j, pt_index1, pt_index2, -3);

                }

                // Create the shaders and buffers
                temp_latitude_circle.set_shader();
                temp_latitude_circle.set_buffer();

                // Add to the latitude circle list
                latitude_outside_circles.Add(temp_latitude_circle);

            }

        }


        public void paint_latitude_circles()
        {

            gvariables_static.LineWidth = 1.5f;

            // Paint the inside latitude circles
            for (int i = 0; i < latitude_inside_circles.Count; i++)
            {
                latitude_inside_circles[i].paint_dynamic_mesh_lines();

            }

            // Paint the outside latitude circles
            for (int i = 0; i < latitude_outside_circles.Count; i++)
            {
                latitude_outside_circles[i].paint_dynamic_mesh_lines();

            }

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of latitude circles

            for (int i = 0; i < latitude_inside_circles.Count; i++)
            {
                latitude_inside_circles[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
                graphic_events_control.geom_transparency);

            }

            for (int i = 0; i < latitude_outside_circles.Count; i++)
            {
                latitude_outside_circles[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
                graphic_events_control.geom_transparency);

            }

        }


        public void update_centerpt(Vector2 new_CenterPt)
        {

            // Update latitude inside circle
            // Latitude inside circles radius interval
            double inside_circle_radius_interval = ((unitcircleradius - centerradius) / (double)(num_of_inside_circles + 1));

            for (int i = 0; i < num_of_inside_circles; i++)
            {
                // Get the circle radius before transformation
                Vector2 t_origin = new Vector2(0.0f, 0.0f);
                double t_circle_radius = centerradius + ((i + 1) * inside_circle_radius_interval);


                if (new_CenterPt != Vector2.Zero)
                {
                    // Find the intersection points of circle and the new_CenterPt vector
                    double newCenterPt_amplitude = Vector2.Distance(new_CenterPt, new Vector2(0, 0));

                    double temp_latitude_circle_radius_ratio = t_circle_radius / newCenterPt_amplitude;

                    Vector2 circle_closest_point = Vector2.Lerp(new Vector2(0, 0), new_CenterPt, (float)temp_latitude_circle_radius_ratio);
                    Vector2 circle_other_point = Vector2.Lerp(new Vector2(0, 0), new_CenterPt, -(float)temp_latitude_circle_radius_ratio);

                    // Transformed circle closest and farther point
                    Vector2 trans_circle_other_pt = gvariables_static.complex_transformation_function(circle_other_point / 1000.0f, new_CenterPt / 1000.0f);
                    Vector2 trans_circle_closest_pt = gvariables_static.complex_transformation_function(circle_closest_point / 1000.0f, new_CenterPt / 1000.0f);

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
                    latitude_inside_circles[i].update_mesh_point(pt_index1, ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    latitude_inside_circles[i].update_mesh_point(pt_index2, ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, -1);

                }

            }


            // Update latitude outside circle
            // Latitude outside circles radius interval
            double outside_circle_radius_interval = ((boundary_size - unitcircleradius) / (double)(num_of_outside_circles + 1));

            for (int i = 0; i < num_of_outside_circles; i++)
            {
                // Get the circle radius before transformation
                Vector2 t_origin = new Vector2(0.0f, 0.0f);
                double t_circle_radius = unitcircleradius + ((i + 1) * outside_circle_radius_interval);


                if (new_CenterPt != new Vector2(0, 0))
                {
                    // Find the intersection points of circle and the new_CenterPt vector
                    double newCenterPt_amplitude = Vector2.Distance(new_CenterPt, new Vector2(0, 0));

                    double temp_latitude_circle_radius_ratio = t_circle_radius / newCenterPt_amplitude;

                    Vector2 circle_closest_point = Vector2.Lerp(new Vector2(0, 0), new_CenterPt, (float)temp_latitude_circle_radius_ratio);
                    Vector2 circle_other_point = Vector2.Lerp(new Vector2(0, 0), new_CenterPt, -(float)temp_latitude_circle_radius_ratio);

                    // Transformed circle closest and farther point
                    Vector2 trans_circle_other_pt = gvariables_static.complex_transformation_function(circle_other_point / 1000.0f, new_CenterPt / 1000.0f);
                    Vector2 trans_circle_closest_pt = gvariables_static.complex_transformation_function(circle_closest_point / 1000.0f, new_CenterPt / 1000.0f);

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
                    latitude_outside_circles[i].update_mesh_point(pt_index1, ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    latitude_outside_circles[i].update_mesh_point(pt_index2, ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, -1);

                }

            }

        }


        private List<Vector2> GetClippedCircle(Vector2 center, double radius, int segment_count)
        {

            // Polygon points
            List<Vector2> PolygonPts = new List<Vector2>();
            PolygonPts.Add(new Vector2(-boundary_size, -boundary_size));
            PolygonPts.Add(new Vector2(-boundary_size, boundary_size));
            PolygonPts.Add(new Vector2(boundary_size, boundary_size));
            PolygonPts.Add(new Vector2(boundary_size, -boundary_size));


            // Step 1: Find intersection points between circle and rectangle edges
            List<(double angle, Vector2 pt)> intersections = getIntersectionsofCircleandPolygon(PolygonPts, center, radius);
            List<Vector2> ClippedCirclePts = new List<Vector2>();

            double angle, x, y;
            int pt_index1, pt_index2;

            if (intersections.Count == 0)
            {
                // Either the circle is inside the rectangle or outside the rectangle 
                // Check whether the circle is outside the rectangle

                if (IsPointInsidePolygon( PolygonPts, new Vector2((float)(center.X + radius), center.Y)) == true)
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

            for (int i = 0; i < intersections.Count; i++)
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
                if (IsPointInsidePolygon(PolygonPts, midPt) == true)
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
            // Not called !!!! but for a fail back on extreme case
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


            return ClippedCirclePts; // return the final clipped polygon points

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


        private List<(double angle, Vector2 pt)> getIntersectionsofCircleandPolygon(List<Vector2> PolygonPts, Vector2 center, double radius)
        {

            List<(double angle, Vector2 pt)> intersections = new List<(double angle, Vector2 pt)>();

            // Loop through the polygon edges

            for (int i = 0; i < PolygonPts.Count; i++)
            {
                // get the line segment start and end point
                Vector2 edge_startPt = PolygonPts[i];
                Vector2 edge_endPt = PolygonPts[(i + 1) % PolygonPts.Count]; // Wrap around

                // Step 1: Circle–Line Equation
                // Circle equation(center
                // c = (cx, cy), radius r:
                // The circle equation is:
                // (x - cx) ^ 2 + (y - cy) ^ 2 = r^2
                // Line segment between 
                // p1 = (x1, y1) and p2 = (x2, y2) can be parameterized as:
                // p(t) = p1​ + t(p2​−p1​), t = [0,1]
                // So: x = x1 + t(x2 - x1), y = y1 + t(y2 - y1)

                // Find the intersection of circle with line segment

                float dx = edge_endPt.X - edge_startPt.X;
                float dy = edge_endPt.Y - edge_startPt.Y;
                float fx = edge_startPt.X - center.X;
                float fy = edge_startPt.Y - center.Y;

                // Quadratic coefficients:
                double a = (dx * dx) + (dy * dy);
                double b = 2 * ((fx * dx) + (fy * dy));
                double c = (fx * fx) + (fy * fy) - (radius * radius);

                // Discriminant:
                double discriminant = (b * b) - (4 * a * c);
                if (discriminant < 0) continue;

                double sqrtDisc = Math.Sqrt(discriminant);
                double t1 = (-b - sqrtDisc) / (2 * a);
                double t2 = (-b + sqrtDisc) / (2 * a);

                foreach (double t in new[] { t1, t2 })
                {
                    if (t >= 0 && t <= 1)
                    {
                        double ix = edge_startPt.X + (t * dx);
                        double iy = edge_startPt.Y + (t * dy);
                        var pt = new Vector2((float)ix, (float)iy);
                        double angle = Math.Atan2(iy - center.Y, ix - center.X);
                        intersections.Add((angle, pt));
                    }
                }

            }

            return intersections;

        }


        private bool IsPointInsidePolygon(List<Vector2> PolygonPts, Vector2 pt)
        {
            // Ray casting method

            // Create a ray of line
            Vector2 ray_endPt = new Vector2(pt.X + 1000000.0f + pt.Y);

            // Intersection count
            int intersection_count = 0;

            for (int i = 0; i < PolygonPts.Count; i++)
            {
                // get the line segment start and end point
                Vector2 edge_startPt = PolygonPts[i];
                Vector2 edge_endPt = PolygonPts[(i + 1) % PolygonPts.Count]; // Wrap around

                // Find whether the line segment formed by pt -> ray_endpt
                // and the second line segement edge_startPt -> edge_endPt
                if (DoSegmentsIntersect(pt, ray_endPt, edge_startPt, edge_endPt))
                {
                    intersection_count++;
                }

            }

            return (intersection_count % 2 == 1);

        }


        private bool DoSegmentsIntersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
        {
            float o1 = Orientation(p1, q1, p2);
            float o2 = Orientation(p1, q1, q2);
            float o3 = Orientation(p2, q2, p1);
            float o4 = Orientation(p2, q2, q1);

            // Only strict intersection, ignore colinear overlap
            return (o1 != o2 && o3 != o4);
        }

        private int Orientation(Vector2 a, Vector2 b, Vector2 c)
        {
            double val = (b.X - a.X) * (c.Y - a.Y) -
                         (b.Y - a.Y) * (c.X - a.X);

            if (Math.Abs(val) < 1e-9) return 0; // treat as colinear
            return (val > 0) ? 1 : 2; // 1 = CCW, 2 = CW
        }



    }
}
