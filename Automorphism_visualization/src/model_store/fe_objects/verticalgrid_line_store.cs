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
    public class verticalgrid_line_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        private int num_outside_grid_lines = 16; // needs to be even to be symmetric
        private int num_inside_grid_lines = 17; // needs to be odd to include origin


        public List<meshdata_store> verticalgrid_inside_lines = new List<meshdata_store>();
        public List<meshdata_store> verticalgrid_outside_lines = new List<meshdata_store>();


        private int segment_count = 60;

        const double unitcircleradius = 1000.0;
        const float boundary_size = 4000.0f;


        public verticalgrid_line_store(double centerradius)
        {

            // Inside lines
            double grid_interval = (2f * unitcircleradius) / (double)(num_inside_grid_lines - 1);

            for (int i = 0; i < num_inside_grid_lines; i++)
            {
                // Vertical grid lines including the origin (inside the circle)
                double x_val = -unitcircleradius + (i * grid_interval);


                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_verticalgrid_line = new meshdata_store();

                // Vertical grid line inside start and end point
                Vector2 temp_startpt = new Vector2((float)x_val,
                    -(float)boundary_size);

                Vector2 temp_endpt = new Vector2((float)x_val,
                    (float)boundary_size);

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

                    temp_verticalgrid_line.add_mesh_point(pt_index1, x, y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    param_t = ((j + 1) / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_verticalgrid_line.add_mesh_point(pt_index2, x, y, 0.0, -1);


                    // Set the line index
                    temp_verticalgrid_line.add_mesh_lines(j, pt_index1, pt_index2, -4);

                }

                // Create the shaders and buffers

                temp_verticalgrid_line.set_shader();
                temp_verticalgrid_line.set_buffer();

                // Add to the list
                verticalgrid_inside_lines.Add(temp_verticalgrid_line);

            }


            // Outside lines
            grid_interval = (2f * (boundary_size - unitcircleradius)) / (double)(num_outside_grid_lines + 1);


            for (int i = 0; i < (int)(num_outside_grid_lines / 2.0f); i++)
            {
                // Vertical grid lines (outside the circle)
                double x_val = -(unitcircleradius + ((i + 1) * grid_interval));

                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_verticalgrid_line1 = new meshdata_store();

                // Vertical grid line inside start and end point
                Vector2 temp_startpt = new Vector2((float)x_val,
                    -(float)boundary_size);

                Vector2 temp_endpt = new Vector2((float)x_val,
                    (float)boundary_size);

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

                    temp_verticalgrid_line1.add_mesh_point(pt_index1, x, y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    param_t = ((j + 1) / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_verticalgrid_line1.add_mesh_point(pt_index2, x, y, 0.0, -1);


                    // Set the line index
                    temp_verticalgrid_line1.add_mesh_lines(j, pt_index1, pt_index2, -4);

                }

                // Create the shaders and buffers

                temp_verticalgrid_line1.set_shader();
                temp_verticalgrid_line1.set_buffer();

                // Add to the list
                verticalgrid_outside_lines.Add(temp_verticalgrid_line1);


                x_val = (unitcircleradius + ((i + 1) * grid_interval));

                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_verticalgrid_line2 = new meshdata_store();

                // Vertical grid line inside start and end point
                temp_startpt = new Vector2((float)x_val,
                    -(float)boundary_size);

                temp_endpt = new Vector2((float)x_val,
                    (float)boundary_size);

                //__________________________________________________________________________
                // Create the Longitude lines
                // Add the boundary points for Longitue lines

                for (int j = 0; j < segment_count; j++)
                {
                    // Create the points for lines
                    // First point 
                    pt_index1 = (2 * j) + 0;
                    param_t = (j / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_verticalgrid_line2.add_mesh_point(pt_index1, x, y, 0.0, -1);

                    // Second point 
                    pt_index2 = (2 * j) + 1;
                    param_t = ((j + 1) / (double)segment_count);
                    x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_verticalgrid_line2.add_mesh_point(pt_index2, x, y, 0.0, -1);


                    // Set the line index
                    temp_verticalgrid_line2.add_mesh_lines(j, pt_index1, pt_index2, -4);

                }

                // Create the shaders and buffers

                temp_verticalgrid_line2.set_shader();
                temp_verticalgrid_line2.set_buffer();

                // Add to the list
                verticalgrid_outside_lines.Add(temp_verticalgrid_line2);

            }

        }



        public void paint_vertical_grid_lines()
        {

            gvariables_static.LineWidth = 1.0f;

            // Paint the inside circle vertical grid lines
            for (int i = 0; i < verticalgrid_inside_lines.Count; i++)
            {
                verticalgrid_inside_lines[i].paint_dynamic_mesh_lines();

            }

            // Paint the outside circle vertical grid lines
            for (int i = 0; i < verticalgrid_outside_lines.Count; i++)
            {
                verticalgrid_outside_lines[i].paint_dynamic_mesh_lines();

            }

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of inside & outside circle vertical grid lines

            for (int i = 0; i < verticalgrid_inside_lines.Count; i++)
            {
                verticalgrid_inside_lines[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
                graphic_events_control.geom_transparency);

            }

            for (int i = 0; i < verticalgrid_outside_lines.Count; i++)
            {
                verticalgrid_outside_lines[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
                graphic_events_control.geom_transparency);

            }

        }


        public void update_centerpt(Vector2 new_CenterPt)
        {

            if (new_CenterPt == Vector2.Zero)
            {
                // Default case (when no centerpoint displacement is given)
                double grid_interval = (2f * unitcircleradius) / (double)(num_inside_grid_lines - 1);

                for (int i = 0; i < num_inside_grid_lines; i++)
                {
                    // Vertical grid lines including the origin (inside the circle)
                    double x_val = -unitcircleradius + (i * grid_interval);

                    // Vertical grid line inside start and end point
                    Vector2 temp_startpt = new Vector2((float)x_val,
                        -(float)boundary_size);

                    Vector2 temp_endpt = new Vector2((float)x_val,
                        (float)boundary_size);


                    //__________________________________________________________________________
                    // Create the Vertical grid lines
                    // Add the boundary points for Vertical grid lines
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

                        verticalgrid_inside_lines[i].update_mesh_point(pt_index1, x, y, 0.0, -1);

                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        param_t = ((j + 1) / (double)segment_count);
                        x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                        y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                        verticalgrid_inside_lines[i].update_mesh_point(pt_index2, x, y, 0.0, -1);

                    }

                }


                // Outside lines
                grid_interval = (2f * (boundary_size - unitcircleradius)) / (double)(num_outside_grid_lines + 1);


                for (int i = 0; i < (int)(num_outside_grid_lines / 2.0f); i++)
                {
                    // Vertical grid lines (outside the circle)
                    double x_val = -(unitcircleradius + ((i + 1) * grid_interval));

                    // Vertical grid line inside start and end point
                    Vector2 temp_startpt = new Vector2((float)x_val,
                        -(float)boundary_size);

                    Vector2 temp_endpt = new Vector2((float)x_val,
                        (float)boundary_size);

                    //__________________________________________________________________________
                    // Create the Vertical grid lines
                    // Add the boundary points for Vertical grid lines
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

                        verticalgrid_outside_lines[(2 * i) + 0].update_mesh_point(pt_index1, x, y, 0.0, -1);

                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        param_t = ((j + 1) / (double)segment_count);
                        x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                        y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                        verticalgrid_outside_lines[(2 * i) + 0].update_mesh_point(pt_index2, x, y, 0.0, -1);

                    }


                    x_val = (unitcircleradius + ((i + 1) * grid_interval));

                    // Vertical grid line inside start and end point
                    temp_startpt = new Vector2((float)x_val,
                        -(float)boundary_size);

                    temp_endpt = new Vector2((float)x_val,
                        (float)boundary_size);

                    //__________________________________________________________________________
                    // Create the Vertical grid lines
                    // Add the boundary points for Vertical grid lines

                    for (int j = 0; j < segment_count; j++)
                    {
                        // Create the points for lines
                        // First point 
                        pt_index1 = (2 * j) + 0;
                        param_t = (j / (double)segment_count);
                        x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                        y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                        verticalgrid_outside_lines[(2 * i) + 1].update_mesh_point(pt_index1, x, y, 0.0, -1);

                        // Second point 
                        pt_index2 = (2 * j) + 1;
                        param_t = ((j + 1) / (double)segment_count);
                        x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                        y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                        verticalgrid_outside_lines[(2 * i) + 1].update_mesh_point(pt_index2, x, y, 0.0, -1);

                    }

                }

            }
            else
            {                 
                // Inside lines
                double grid_interval = (2f * unitcircleradius) / (double)(num_inside_grid_lines - 1);

                for (int i = 0; i < num_inside_grid_lines; i++)
                {
                    // Vertical grid lines including the origin (inside the circle)
                    double x_val = -unitcircleradius + (i * grid_interval);

                    // Vertical grid line inside start and end point
                    Vector2 temp_startpt = new Vector2((float)x_val,
                        -(float)boundary_size);

                    Vector2 temp_endpt = new Vector2((float)x_val,
                        (float)boundary_size);


                    // Collinear check
                    if (IsPointsCollinear(temp_startpt, temp_endpt, new_CenterPt) == false)
                    {
                        
                        Vector2 temp_midpt = new Vector2((float)x_val, 0.0f);

                        // Get the 3 point arcs
                        Vector2 arc_startpt = gvariables_static.complex_transformation_function(temp_startpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;

                        Vector2 arc_midpt = gvariables_static.complex_transformation_function(temp_midpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;

                        Vector2 arc_endpt = gvariables_static.complex_transformation_function(temp_endpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;


                        Vector2 arc_center = FindCircleCenter(arc_startpt, arc_midpt, arc_endpt);
                        double arc_radius = Vector2.Distance(arc_center, arc_startpt);

                        int pt_index1, pt_index2;
                        List<Vector2> ClippedCirclePts = GetClippedCircle(arc_center, arc_radius, segment_count);


                        for (int j = 0; j < segment_count; j++)
                        {
                            // Normalized deflection scale
                            double normalized_defl_scale = Vector2.Distance(temp_startpt + ((float)(j / (float)(segment_count - 1)) * temp_endpt), 
                                Vector2.Zero) /boundary_size;

                            // Create the points for circle segments
                            // First point 
                            pt_index1 = (2 * j) + 0;
                            verticalgrid_inside_lines[i].update_mesh_point(pt_index1,
                                ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, normalized_defl_scale);

                            // Second point 
                            pt_index2 = (2 * j) + 1;
                            verticalgrid_inside_lines[i].update_mesh_point(pt_index2,
                                ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, normalized_defl_scale);

                        }

                    }

                }


                // Outside lines
                grid_interval = (2f * (boundary_size - unitcircleradius)) / (double)(num_outside_grid_lines + 1);


                for (int i = 0; i < (int)(num_outside_grid_lines / 2.0f); i++)
                {
                    // Vertical grid lines (outside the circle)
                    double x_val = -(unitcircleradius + ((i + 1) * grid_interval));

                    // Vertical grid line inside start and end point
                    Vector2 temp_startpt = new Vector2((float)x_val,
                        -(float)boundary_size);

                    Vector2 temp_endpt = new Vector2((float)x_val,
                        (float)boundary_size);


                    // Collinear check
                    if (IsPointsCollinear(temp_startpt, temp_endpt, new_CenterPt) == false)
                    {
       
                        Vector2 temp_midpt = new Vector2((float)x_val, 0.0f);

                        // Get the 3 point arcs
                        Vector2 arc_startpt = gvariables_static.complex_transformation_function(temp_startpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;

                        Vector2 arc_midpt = gvariables_static.complex_transformation_function(temp_midpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;

                        Vector2 arc_endpt = gvariables_static.complex_transformation_function(temp_endpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;


                        Vector2 arc_center = FindCircleCenter(arc_startpt, arc_midpt, arc_endpt);
                        double arc_radius = Vector2.Distance(arc_center, arc_startpt);

                        // Outside lines 1
                        int pt_index1, pt_index2;
                        List<Vector2> ClippedCirclePts = GetClippedCircle(arc_center, arc_radius, segment_count);


                        for (int j = 0; j < segment_count; j++)
                        {
                            // Normalized deflection scale
                            double normalized_defl_scale = Vector2.Distance(temp_startpt + ((float)(j / (float)(segment_count - 1)) * temp_endpt),
                                Vector2.Zero) / boundary_size;

                            // Create the points for circle segments
                            // First point 
                            pt_index1 = (2 * j) + 0;
                            verticalgrid_outside_lines[(2 * i) + 0].update_mesh_point(pt_index1,
                                ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, normalized_defl_scale);

                            // Second point 
                            pt_index2 = (2 * j) + 1;
                            verticalgrid_outside_lines[(2 * i) + 0].update_mesh_point(pt_index2,
                                ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, normalized_defl_scale);

                        }

                    }


                    x_val = (unitcircleradius + ((i + 1) * grid_interval));

                    // Vertical grid line inside start and end point
                    temp_startpt = new Vector2((float)x_val,
                        -(float)boundary_size);

                    temp_endpt = new Vector2((float)x_val,
                        (float)boundary_size);


                    // Collinear check
                    if (IsPointsCollinear(temp_startpt, temp_endpt, new_CenterPt) == false)
                    {
       
                        Vector2 temp_midpt = new Vector2((float)x_val, 0.0f);

                        // Get the 3 point arcs
                        Vector2 arc_startpt = gvariables_static.complex_transformation_function(temp_startpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;

                        Vector2 arc_midpt = gvariables_static.complex_transformation_function(temp_midpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;

                        Vector2 arc_endpt = gvariables_static.complex_transformation_function(temp_endpt / 1000.0f,
                            new_CenterPt / 1000.0f) * 1000.0f;


                        Vector2 arc_center = FindCircleCenter(arc_startpt, arc_midpt, arc_endpt);
                        double arc_radius = Vector2.Distance(arc_center, arc_startpt);


                        // Outside lines 2
                        int pt_index1, pt_index2;
                        List<Vector2> ClippedCirclePts = GetClippedCircle(arc_center, arc_radius, segment_count);


                        for (int j = 0; j < segment_count; j++)
                        {
                            // Normalized deflection scale
                            double normalized_defl_scale = Vector2.Distance(temp_startpt + ((float)(j / (float)(segment_count - 1)) * temp_endpt),
                                Vector2.Zero) / boundary_size;

                            // Create the points for circle segments
                            // First point 
                            pt_index1 = (2 * j) + 0;
                            verticalgrid_outside_lines[(2 * i) + 1].update_mesh_point(pt_index1,
                                ClippedCirclePts[pt_index1].X, ClippedCirclePts[pt_index1].Y, 0.0, normalized_defl_scale);

                            // Second point 
                            pt_index2 = (2 * j) + 1;
                            verticalgrid_outside_lines[(2 * i) + 1].update_mesh_point(pt_index2,
                                ClippedCirclePts[pt_index2].X, ClippedCirclePts[pt_index2].Y, 0.0, normalized_defl_scale);

                        }

                    }

                }

            }


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


        private List<Vector2> GetClippedCircle(Vector2 center, double radius, int segment_count)
        {

            // Polygon points
            List<Vector2> PolygonPts = new List<Vector2>();
            PolygonPts.Add(new Vector2(-boundary_size, -boundary_size));
            PolygonPts.Add(new Vector2(-boundary_size, boundary_size));
            PolygonPts.Add(new Vector2(boundary_size, boundary_size));
            PolygonPts.Add(new Vector2(boundary_size, -boundary_size));


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
            const float xmin = -boundary_size, xmax = boundary_size, ymin = -boundary_size, ymax = boundary_size;

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
            const float xmin = -boundary_size, xmax = boundary_size, ymin = -boundary_size, ymax = boundary_size;
            return pt.X > xmin && pt.X < xmax && pt.Y > ymin && pt.Y < ymax;

        }








        private bool IsPointsCollinear(Vector2 pt1, Vector2 pt2, Vector2 pt3)
        {
            double cross = (pt2.X - pt1.X) * (pt3.Y - pt1.Y) -
                                   (pt2.Y - pt1.Y) * (pt3.X - pt1.X);
            const double epsilon = 10.0;
            return Math.Abs(cross) < epsilon;

        }





    }
}
