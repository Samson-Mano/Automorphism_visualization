using _2DHelmholtz_solver.global_variables;
using _2DHelmholtz_solver.src.model_store.geom_objects;
using _2DHelmholtz_solver.src.opentk_control.opentk_bgdraw;
using OpenTK;
using SharpFont.PostScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automorphism_visualization.src.model_store.fe_objects
{
    public class longitude_line_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        private int num_inside_lines = 16;
        private int num_outside_lines = 16;

        public List<meshdata_store> longitude_inside_lines = new List<meshdata_store>();
        public List<meshdata_store> longitude_outside_lines = new List<meshdata_store>();


        private int segment_count = 30;

        private double unitcircleradius = 1000.0;
        const float boundary_size = 4000.0f;



        public longitude_line_store(double centerradius)
        {
            // center radius
            this.centerradius = centerradius;

            // Longitude inside lines angle interval
            double angle_interval1 = (2.0 * Math.PI) / (double)num_inside_lines;

            // Inside lines
            for (int i = 0; i < num_inside_lines; i++)
            {
                // Temporary lines angle
                double temp_logitude_angle = i * angle_interval1;


                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_longitude_line = new meshdata_store();

                // Longitude line start and end point
                Vector2 temp_startpt = new Vector2(0.0f + (float)(centerradius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(centerradius * Math.Sin(temp_logitude_angle)));

                Vector2 temp_endpt = new Vector2(0.0f + (float)(unitcircleradius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(unitcircleradius * Math.Sin(temp_logitude_angle)));

                //__________________________________________________________________________
                // Create the Longitude lines
                // Add the boundary points for Longitue lines
                double param_t, x, y;
                int pt_index1, pt_index2;

                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for lines
                    // First point 
                    pt_index1 = (2 * j) + 0;
                    param_t = (j / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_longitude_line.add_mesh_point(pt_index1, x, y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    param_t = ((j + 1) / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_longitude_line.add_mesh_point(pt_index2, x, y, 0.0, -1);


                    // Set the line index
                    temp_longitude_line.add_mesh_lines(j, pt_index1, pt_index2, -4);

                }

                // Create the shaders and buffers

                temp_longitude_line.set_shader();
                temp_longitude_line.set_buffer();

                // Add to the list
                longitude_inside_lines.Add(temp_longitude_line);

            }

            // Longitude outside lines angle interval
            double angle_interval2 = (2.0 * Math.PI) / (double)num_outside_lines;

            // Outside lines
            for (int i = 0; i < num_outside_lines; i++)
            {
                // Temporary lines angle
                double temp_logitude_angle = i * angle_interval2;


                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_longitude_line = new meshdata_store();

                // find the outtermost outside circle radius
                int num_of_outside_circles = 10;
                double outside_circle_radius = unitcircleradius +
                    ((boundary_size - unitcircleradius) * (num_of_outside_circles / (double)(num_of_outside_circles + 1)));

                // Longitude line start and end point
                Vector2 temp_startpt = new Vector2(0.0f + (float)(outside_circle_radius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(outside_circle_radius * Math.Sin(temp_logitude_angle)));

                Vector2 temp_endpt = new Vector2(0.0f + (float)(unitcircleradius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(unitcircleradius * Math.Sin(temp_logitude_angle)));

                //__________________________________________________________________________
                // Create the Longitude lines
                // Add the boundary points for Longitue lines
                double param_t, x, y;
                int pt_index1, pt_index2;

                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for lines
                    // First point 
                    pt_index1 = (2 * j) + 0;
                    param_t = (j / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_longitude_line.add_mesh_point(pt_index1, x, y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    param_t = ((j + 1) / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_longitude_line.add_mesh_point(pt_index2, x, y, 0.0, -1);


                    // Set the line index
                    temp_longitude_line.add_mesh_lines(j, pt_index1, pt_index2, -4);

                }

                // Create the shaders and buffers

                temp_longitude_line.set_shader();
                temp_longitude_line.set_buffer();

                // Add to the list
                longitude_outside_lines.Add(temp_longitude_line);

            }



        }



        public void paint_longitude_lines()
        {
            // Paint the center circle
            gvariables_static.LineWidth = 1.5f;

            // Inside lines
            for (int i = 0; i < longitude_inside_lines.Count; i++)
            {
                longitude_inside_lines[i].paint_dynamic_mesh_lines();

            }

            // Outside lines
            for (int i = 0; i < longitude_outside_lines.Count; i++)
            {
                longitude_outside_lines[i].paint_dynamic_mesh_lines();

            }

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of center circle
            // Inside circles
            for (int i = 0; i < longitude_inside_lines.Count; i++)
            {
                longitude_inside_lines[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);

            }

            // Outside circles
            for (int i = 0; i < longitude_outside_lines.Count; i++)
            {
                longitude_outside_lines[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);

            }

        }


        public void update_centerpt(Vector2 new_CenterPt)
        {

            // Longitude lines angle interval
            double angle_interval = (2.0 * Math.PI) / (double)num_inside_lines;

            double angle_start = 0.0;

            if (new_CenterPt != Vector2.Zero)
            {
                // Create the angle start
                angle_start = Math.Atan2(new_CenterPt.Y, new_CenterPt.X);

                // Convert it to 0 to 2PI
                if (angle_start < 0)
                {
                    angle_start += 2 * Math.PI;
                }

            }

            // Inside lines
            update_inside_lines(new_CenterPt, angle_start, angle_interval);


            // Outside lines
            for (int i = 0; i < num_outside_lines; i++)
            {
                // Temporary lines angle
                double temp_logitude_angle = angle_start + (i * angle_interval);

                if (temp_logitude_angle > (2 * Math.PI))
                {
                    temp_logitude_angle -= 2 * Math.PI;
                }

                // find the outtermost outside circle radius
                int num_of_outside_circles = 10;
                double outside_circle_radius = unitcircleradius +
                    ((boundary_size - unitcircleradius) * (num_of_outside_circles / (double)(num_of_outside_circles + 1)));

                // Longitude line start and end point
                Vector2 temp_startpt = new Vector2(0.0f + (float)(outside_circle_radius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(outside_circle_radius * Math.Sin(temp_logitude_angle)));

                Vector2 temp_endpt = new Vector2(0.0f + (float)(unitcircleradius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(unitcircleradius * Math.Sin(temp_logitude_angle)));


                // Check whether the arc points are collinear
                if (IsPointsCollinear(temp_startpt, temp_endpt, new_CenterPt) == true)
                {
                    //__________________________________________________________________________
                    // Create the Longitude lines
                    // Add the boundary points for Longitue lines
                    double param_t, x, y;
                    int pt_index1, pt_index2;

                    for (int j = 0; j < segment_count; j++)
                    {
                        // Create the points for lines
                        // First point 
                        pt_index1 = (2 * j) + 0;
                        param_t = (j / (double)segment_count);
                        x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                        y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                        longitude_outside_lines[i].update_mesh_point(pt_index1, -4000.0f, -4000.0f, 0.0, -1);

                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        param_t = ((j + 1) / (double)segment_count);
                        x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                        y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                        longitude_outside_lines[i].update_mesh_point(pt_index2, -4000.0f, -4000.0f, 0.0, -1);

                    }

                }
                else
                {
                    Vector2 temp_midpt = (temp_startpt + temp_endpt) * 0.5f;


                    // Get the 3 point arcs
                    Vector2 arc_startpt = gvariables_static.complex_transformation_function(temp_startpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 arc_midpt = gvariables_static.complex_transformation_function(temp_midpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 arc_endpt = gvariables_static.complex_transformation_function(temp_endpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;



                    Vector2 arc_center = FindCircleCenter(arc_startpt, arc_midpt, arc_endpt);
                    double arc_radius = Vector2.Distance(arc_center, arc_startpt);

                    // Get the polygons which contains the arc segment
                    List<Vector2> PolygonPts = GetArcSectionPolygonPts(arc_startpt, arc_endpt, arc_midpt);

                    int pt_index1, pt_index2;
                    List<Vector2> ClippedCirclePts = GetClippedCircle(PolygonPts, arc_center, arc_radius, segment_count);

                    for (int j = 0; j < segment_count; j++)
                    {
                        // Create the points for lines
                        // First point 
                        pt_index1 = (2 * j) + 0;
                        longitude_outside_lines[i].update_mesh_point(pt_index1, ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, -1);


                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        longitude_outside_lines[i].update_mesh_point(pt_index2, ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, -1);

                    }

                }

            }

        }



        private void update_inside_lines(Vector2 new_CenterPt, double angle_start, double angle_interval)
        {

            // Inside lines
            for (int i = 0; i < num_inside_lines; i++)
            {
                // Temporary lines angle
                double temp_logitude_angle = angle_start + (i * angle_interval);

                if (temp_logitude_angle > (2 * Math.PI))
                {
                    temp_logitude_angle -= 2 * Math.PI;
                }

                // Longitude line start and end point
                Vector2 temp_startpt = new Vector2(0.0f + (float)(centerradius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(centerradius * Math.Sin(temp_logitude_angle)));

                Vector2 temp_endpt = new Vector2(0.0f + (float)(unitcircleradius * Math.Cos(temp_logitude_angle)),
                    0.0f + (float)(unitcircleradius * Math.Sin(temp_logitude_angle)));


                // Check whether the arc points are collinear
                if (IsPointsCollinear(temp_startpt, temp_endpt, new_CenterPt) == true)
                {

                    Vector2 temp_startpt01 = (0.001f * temp_startpt) + ((1.0f - 0.001f) * temp_endpt);
                    Vector2 temp_endpt01 = (0.001f * temp_endpt) + ((1.0f - 0.001f) * temp_startpt);

                    // Apply transformation to straight line start point and end point
                    Vector2 straightline_startpt = gvariables_static.complex_transformation_function(temp_startpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 straightline_startpt01 = gvariables_static.complex_transformation_function(temp_startpt01 / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 straightline_endpt01 = gvariables_static.complex_transformation_function(temp_endpt01 / 1000.0f,
                         new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 straightline_endpt = gvariables_static.complex_transformation_function(temp_endpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;



                    if (IsPointsOrdered(straightline_startpt, straightline_startpt01, straightline_endpt) == true)
                    {
                        // Straight line which directly goes to end
                        // Straight line
                        double param_t, x, y;
                        int pt_index1, pt_index2;
                        Vector2 intersection_pt = straightline_startpt;

                        if (GetIntersectionWithRectangle(straightline_startpt, straightline_endpt, out Vector2 intersection) == true)
                        {
                            intersection_pt = intersection;
                        }

                        for (int j = 0; j < segment_count; j++)
                        {
                            // Create the points for lines
                            // First point 
                            pt_index1 = (2 * j) + 0;
                            param_t = (j / (double)segment_count);
                            x = (1.0 - param_t) * intersection_pt.X + (param_t * straightline_endpt.X);
                            y = (1.0 - param_t) * intersection_pt.Y + (param_t * straightline_endpt.Y);

                            longitude_inside_lines[i].update_mesh_point(pt_index1, (float)x, (float)y, 0.0, -1);


                            // Second point 
                            pt_index2 = (2 * j) + 1;
                            param_t = ((j + 1) / (double)segment_count);
                            x = (1.0 - param_t) * intersection_pt.X + (param_t * straightline_endpt.X);
                            y = (1.0 - param_t) * intersection_pt.Y + (param_t * straightline_endpt.Y);

                            longitude_inside_lines[i].update_mesh_point(pt_index2, (float)x, (float)y, 0.0, -1);

                        }

                    }
                    else
                    {
                        // Straight line which wraps around the infinity to go to end

                        // Straight line which goes to onside of infinity
                        Vector2 linDir_pinf = straightline_startpt01 - straightline_startpt;
                        Vector2 intersection_pt_pinf = straightline_startpt + Vector2.Normalize(linDir_pinf) * 1000000.0f;
                        double length_pinf = 0.0;

                        if (GetIntersectionWithRectangle(straightline_startpt, intersection_pt_pinf, out Vector2 intersection1) == true)
                        {
                            intersection_pt_pinf = intersection1;
                            length_pinf = Vector2.Distance(straightline_startpt, intersection_pt_pinf);
                        }

                        // Straight line which goes to other side of infinity
                        Vector2 linDir_ninf = straightline_endpt01 - straightline_endpt;
                        Vector2 intersection_pt_ninf = straightline_endpt + Vector2.Normalize(linDir_ninf) * 1000000.0f;
                        double length_ninf = 0.0;

                        if (GetIntersectionWithRectangle(straightline_endpt, intersection_pt_ninf, out Vector2 intersection2) == true)
                        {
                            intersection_pt_ninf = intersection2;
                            length_ninf = Vector2.Distance(straightline_endpt, intersection_pt_ninf);
                        }


                        double total_length = length_pinf + length_ninf;

                        int first_segment_count = (int)Math.Round((length_pinf / total_length) * segment_count);
                        int second_segment_count = segment_count - first_segment_count;


                        for (int j = 0; j < segment_count; j++)
                        {
                            double param_t, x, y;
                            int pt_index1, pt_index2;

                            if(j < first_segment_count)
                            {
                                // 1) Towards the one side of infinity
                                // Create the points for lines
                                // First point 
                                pt_index1 = (2 * j) + 0;
                                param_t = (j / (double)first_segment_count);
                                x = (1.0 - param_t) * intersection_pt_pinf.X + (param_t * straightline_endpt.X);
                                y = (1.0 - param_t) * intersection_pt_pinf.Y + (param_t * straightline_endpt.Y);

                                longitude_inside_lines[i].update_mesh_point(pt_index1, (float)x, (float)y, 0.0, -1);


                                // Second point 
                                pt_index2 = (2 * j) + 1;
                                param_t = ((j + 1) / (double)first_segment_count);
                                x = (1.0 - param_t) * intersection_pt_pinf.X + (param_t * straightline_endpt.X);
                                y = (1.0 - param_t) * intersection_pt_pinf.Y + (param_t * straightline_endpt.Y);

                                longitude_inside_lines[i].update_mesh_point(pt_index2, (float)x, (float)y, 0.0, -1);
                            }
                            else
                            {

                                // 2) Towards the other side of inifinity
                                // Create the points for lines
                                // First point 
                                pt_index1 = (2 * j) + 0;
                                param_t = ((j - first_segment_count) / (double)second_segment_count);
                                x = (1.0 - param_t) * intersection_pt_ninf.X + (param_t * straightline_startpt.X);
                                y = (1.0 - param_t) * intersection_pt_ninf.Y + (param_t * straightline_startpt.Y);

                                longitude_inside_lines[i].update_mesh_point(pt_index1, (float)x, (float)y, 0.0, -1);


                                // Second point 
                                pt_index2 = (2 * j) + 1;
                                param_t = ((j - first_segment_count + 1) / (double)second_segment_count);
                                x = (1.0 - param_t) * intersection_pt_ninf.X + (param_t * straightline_startpt.X);
                                y = (1.0 - param_t) * intersection_pt_ninf.Y + (param_t * straightline_startpt.Y);

                                longitude_inside_lines[i].update_mesh_point(pt_index2, (float)x, (float)y, 0.0, -1);

                            }

                        }

                    }

                }
                else
                {


                    Vector2 temp_midpt = (temp_startpt + temp_endpt) * 0.5f;


                    // Get the 3 point arcs
                    Vector2 arc_startpt = gvariables_static.complex_transformation_function(temp_startpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 arc_midpt = gvariables_static.complex_transformation_function(temp_midpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;

                    Vector2 arc_endpt = gvariables_static.complex_transformation_function(temp_endpt / 1000.0f,
                        new_CenterPt / 1000.0f) * 1000.0f;



                    Vector2 arc_center = FindCircleCenter(arc_startpt, arc_midpt, arc_endpt);
                    double arc_radius = Vector2.Distance(arc_center, arc_startpt);

                    // Get the polygons which contains the arc segment
                    List<Vector2> PolygonPts = GetArcSectionPolygonPts(arc_startpt, arc_endpt, arc_midpt);

                    int pt_index1, pt_index2;
                    List<Vector2> ClippedCirclePts = GetClippedCircle(PolygonPts, arc_center, arc_radius, segment_count);

                    for (int j = 0; j < segment_count; j++)
                    {
                        // Create the points for lines
                        // First point 
                        pt_index1 = (2 * j) + 0;
                        longitude_inside_lines[i].update_mesh_point(pt_index1, ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, -1);


                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        longitude_inside_lines[i].update_mesh_point(pt_index2, ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, -1);

                    }

                }

            }

        }



        private void update_outside_lines(Vector2 new_CenterPt, double angle_start, double angle_interval)
        {



        }





        private Vector2 FindCircleCenter(Vector2 A, Vector2 B, Vector2 C)
        {
            // Midpoints
            Vector2 midAB = (A + B) / 2;
            Vector2 midBC = (B + C) / 2;

            // Slopes
            Vector2 dirAB = B - A;
            Vector2 dirBC = C - B;

            // Perpendicular directions
            Vector2 perpAB = new Vector2(-dirAB.Y, dirAB.X);
            Vector2 perpBC = new Vector2(-dirBC.Y, dirBC.X);

            // Solve for intersection of lines: midAB + t1 * perpAB = midBC + t2 * perpBC
            // Using linear algebra or substitution
            float a1 = perpAB.X;
            float b1 = -perpBC.X;
            float c1 = midBC.X - midAB.X;

            float a2 = perpAB.Y;
            float b2 = -perpBC.Y;
            float c2 = midBC.Y - midAB.Y;

            float denom = a1 * b2 - a2 * b1;
            if (Math.Abs(denom) < 1e-6)
                return new Vector2(float.MaxValue, float.MaxValue);
            //throw new Exception("Points are colinear or too close to form a valid arc.");

            float t1 = (c1 * b2 - c2 * b1) / denom;

            return midAB + t1 * perpAB;
        }


        private List<Vector2> GetArcSectionPolygonPts(Vector2 arc_startPt, Vector2 arc_endPt, Vector2 arc_midPt)
        {

            // Rectangle Boundary points
            // Define rectangle (CCW order)
            List<Vector2> rect = new List<Vector2>
    {
        new Vector2(-boundary_size, -boundary_size),
        new Vector2(-boundary_size,  boundary_size),
        new Vector2( boundary_size,  boundary_size),
        new Vector2( boundary_size, -boundary_size)
    };

            // Step 1: Create a line from arc_startPt to arc_endPt
            Vector2 lineDir = (arc_endPt - arc_startPt).Normalized();
            Vector2 normal = new Vector2(-lineDir.Y, lineDir.X); // Perpendicular vector

            // Step 2: Classify each rectangle point as being on one side of the line or the other
            List<Vector2> sideA = new List<Vector2>();
            List<Vector2> sideB = new List<Vector2>();

            foreach (var pt in rect)
            {
                float side = Vector2.Dot(pt - arc_startPt, normal);
                if (Math.Abs(side) < 1e-6f)
                {
                    // On the line → add to both sides
                    sideA.Add(pt);
                    sideB.Add(pt);
                }
                else if (side > 0)
                    sideA.Add(pt);
                else
                    sideB.Add(pt);
            }


            // Step 3: Add intersection points of the line with the rectangle edges
            // Create a line ray from arc start point and arc end point

            Vector2 arc_points_direction = (arc_endPt - arc_startPt).Normalized();
            Vector2 extended_arc_startPt = arc_startPt - arc_points_direction * 1000000f;
            Vector2 extended_arc_endPt = arc_endPt + arc_points_direction * 1000000f;

            List<Vector2> intersections = GetLineRectangleIntersections(extended_arc_startPt, extended_arc_endPt, rect);

            sideA.AddRange(intersections);
            sideB.AddRange(intersections);

            // Step 4: Form a CCW convex hull for SideA and SideB
            List<Vector2> polygonA = ComputeConvexHullCCW(sideA);
            List<Vector2> polygonB = ComputeConvexHullCCW(sideB);


            // Step 5: Determine which side contains arc_midPt
            // Get the arc_midPt direction
            Vector2 arc_midPtDir = (arc_midPt - (arc_startPt + arc_endPt) * 0.5f).Normalized();

            // Arc end Point is strictly inside the rectangle therefore use shifted arc end point to test
            Vector2 shifted_arcendPt = arc_endPt + arc_midPtDir;

            return IsPointInsidePolygon(polygonA, shifted_arcendPt) ? polygonA : polygonB;

        }


        private List<Vector2> ComputeConvexHullCCW(List<Vector2> points)
        {
            if (points.Count <= 3)
                return new List<Vector2>(points); // Already a polygon

            // Sort points lexicographically (by X, then Y)
            points.Sort((a, b) =>
            {
                int cmpX = a.X.CompareTo(b.X);
                return (cmpX != 0) ? cmpX : a.Y.CompareTo(b.Y);
            });

            List<Vector2> lower = new List<Vector2>();
            foreach (var p in points)
            {
                while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            List<Vector2> upper = new List<Vector2>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                var p = points[i];
                while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }

            // Remove last point of each half (it's duplicated)
            upper.RemoveAt(upper.Count - 1);
            lower.RemoveAt(lower.Count - 1);

            // Concatenate lower and upper to get full hull
            lower.AddRange(upper);
            return lower;
        }

        private float Cross(Vector2 o, Vector2 a, Vector2 b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }


        private List<Vector2> GetLineRectangleIntersections(Vector2 p1, Vector2 p2, List<Vector2> rect)
        {
            List<Vector2> intersections = new List<Vector2>();

            for (int i = 0; i < rect.Count; i++)
            {
                Vector2 r1 = rect[i];
                Vector2 r2 = rect[(i + 1) % rect.Count];

                if (DoSegmentsIntersect(p1, p2, r1, r2, out Vector2 intersection))
                {
                    intersections.Add(intersection);
                }
            }

            return intersections;
        }



        private List<Vector2> GetClippedCircle(List<Vector2> PolygonPts, Vector2 center, double radius, int segment_count)
        {

            // Step 1: Find intersection points between circle and rectangle edges
            List<(double angle, Vector2 pt)> intersections = getIntersectionsofCircleandPolygon(PolygonPts, center, radius);
            List<Vector2> ClippedCirclePts = new List<Vector2>();

            double angle, x, y;
            int pt_index1, pt_index2;

            if (intersections.Count == 0)
            {
                // Either the circle is inside the rectangle or outside the rectangle 
                // Check whether the circle is outside the rectangle

                if (IsPointInsidePolygon(PolygonPts, new Vector2((float)(center.X + radius), center.Y)) == true)
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


        private bool DoSegmentsIntersect(Vector2 lineStart, Vector2 lineEnd, Vector2 edgeStart, Vector2 edgeEnd)
        {
            float o1 = Orientation(lineStart, lineEnd, edgeStart);
            float o2 = Orientation(lineStart, lineEnd, edgeEnd);
            float o3 = Orientation(edgeStart, edgeEnd, lineStart);
            float o4 = Orientation(edgeStart, edgeEnd, lineEnd);

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


        private bool DoSegmentsIntersect(Vector2 lineStart, Vector2 lineEnd, Vector2 edgeStart, Vector2 edgeEnd, out Vector2 intersection)
        {
            intersection = new Vector2();

            float o1 = Orientation(lineStart, lineEnd, edgeStart);
            float o2 = Orientation(lineStart, lineEnd, edgeEnd);
            float o3 = Orientation(edgeStart, edgeEnd, lineStart);
            float o4 = Orientation(edgeStart, edgeEnd, lineEnd);

            // Check if segments intersect (strictly, no colinear overlap)
            if (o1 != o2 && o3 != o4)
            {
                // Line AB represented as a1x + b1y = c1
                float a1 = lineEnd.Y - lineStart.Y;
                float b1 = lineStart.X - lineEnd.X;
                float c1 = (a1 * lineStart.X) + (b1 * lineStart.Y);

                // Line CD represented as a2x + b2y = c2
                float a2 = edgeEnd.Y - edgeStart.Y;
                float b2 = edgeStart.X - edgeEnd.X;
                float c2 = (a2 * edgeStart.X) + (b2 * edgeStart.Y);

                float determinant = (a1 * b2) - (a2 * b1);

                if (Math.Abs(determinant) > 1e-6)
                {
                    float x = ((b2 * c1) - (b1 * c2)) / determinant;
                    float y = ((a1 * c2) - (a2 * c1)) / determinant;
                    intersection = new Vector2(x, y);
                    return true;
                }
            }

            return false;
        }


        private bool IsPointsCollinear(Vector2 pt1, Vector2 pt2, Vector2 pt3)
        {
            double cross = (pt2.X - pt1.X) * (pt3.Y - pt1.Y) -
                                   (pt2.Y - pt1.Y) * (pt3.X - pt1.X);
            const double epsilon = 10.0;
            return Math.Abs(cross) < epsilon;

        }


        private bool IsPointsOrdered(Vector2 pt1, Vector2 pt2, Vector2 pt3)
        {
            // Vector from pt1 to pt2
            Vector2 v1 = pt2 - pt1;

            // Vector from pt2 to pt3
            Vector2 v2 = pt3 - pt2;

            // Check if the dot product is positive
            // This means the angle between v1 and v2 is less than 90 degrees
            return Vector2.Dot(v1, v2) > 0;
        }



        private bool GetIntersectionWithRectangle(Vector2 lineStart, Vector2 lineEnd, out Vector2 intersection)
        {
            intersection = default;

            // Rectangle boundaries
            List<Vector2> RectanglePts = new List<Vector2>()
            {
                new Vector2(-boundary_size, -boundary_size),
                new Vector2(-boundary_size, boundary_size),
                new Vector2(boundary_size, boundary_size),
                new Vector2(boundary_size, -boundary_size)
            };


            for (int i = 0; i < RectanglePts.Count; i++)
            {
                Vector2 edgeStart = RectanglePts[i];
                Vector2 edgeEnd = RectanglePts[(i + 1) % RectanglePts.Count]; // wrap around


                if (DoSegmentsIntersect(lineStart, lineEnd, edgeStart, edgeEnd, out intersection) == true)
                    return true;
            }

            return false;

        }

    }
}
