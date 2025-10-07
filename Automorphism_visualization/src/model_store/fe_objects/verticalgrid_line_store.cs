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
                meshdata_store temp_longitude_line = new meshdata_store();

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
                verticalgrid_inside_lines.Add(temp_longitude_line);

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

            gvariables_static.LineWidth = 1.5f;

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


        }



    }
}
