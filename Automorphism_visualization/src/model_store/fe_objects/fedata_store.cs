using _2DHelmholtz_solver.global_variables;
using _2DHelmholtz_solver.src.events_handler;
using _2DHelmholtz_solver.src.model_store.geom_objects;
using _2DHelmholtz_solver.src.opentk_control.opentk_bgdraw;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Automorphism_visualization.src.model_store.fe_objects;


namespace _2DHelmholtz_solver.src.model_store.fe_objects
{


    public class fedata_store
    {

        public meshdata_store shaded_meshdata;
        public meshdata_store boundary_lines;
        public center_circle_store center_circle;


        public bool isModelSet = false;


        // Drawing bound data
        public Vector3 min_bounds = new Vector3(-1);
        public Vector3 max_bounds = new Vector3(1);
        public Vector3 geom_bounds = new Vector3(2);


        public selectrectangle_store selection_rectangle { get; }
        public selectcircle_store selection_circle { get; }

        // To control the drawing events
        public drawing_events graphic_events_control { get; private set; }



        public fedata_store()
        {
            // (Re)Initialize the data
            shaded_meshdata = new meshdata_store();
            boundary_lines = new meshdata_store();

            // To control the drawing graphics
            graphic_events_control = new drawing_events(this);

            // Set the selection rectangle  & selection circle
            selection_rectangle = new selectrectangle_store();
            selection_circle = new selectcircle_store();

            // Temporary Geometry bounds
            this.min_bounds = new Vector3(-1);
            this.max_bounds = new Vector3(1);
            this.geom_bounds = new Vector3(2);


        }

        public void importMesh()
        {
            List<Vector3> nodePtsList = new List<Vector3>();
            isModelSet = false;

            // Boundary of the circles
            float circle_size = 1000.0f;

            nodePtsList.Add(new Vector3(-circle_size, -circle_size, 0.0f));
            nodePtsList.Add(new Vector3(-circle_size, circle_size, 0.0f));
            nodePtsList.Add(new Vector3(circle_size, circle_size, 0.0f));
            nodePtsList.Add(new Vector3(circle_size, -circle_size, 0.0f));

            node_list_store temp_nodes = new node_list_store();
            elementtri_list_store temp_tris = new elementtri_list_store();
            elementquad_list_store temp_quads = new elementquad_list_store();


            file_events.import_mesh(ref temp_nodes, ref temp_tris, ref temp_quads, ref isModelSet);


            if (isModelSet == false)
                return;

            // Set the mesh boundaries
            Vector3 geometry_center = gvariables_static.FindGeometricCenter(nodePtsList);
            Tuple<Vector3, Vector3> geom_extremes = gvariables_static.FindMinMaxXY(nodePtsList);

            Vector3 geom_min_b = geom_extremes.Item1; // Minimum bound
            Vector3 geom_max_b = geom_extremes.Item2; // Maximum bound

            Vector3 geom_bounds = geom_max_b - geom_min_b;


            // Set the FE Model boundary
            this.min_bounds = geom_min_b;
            this.max_bounds = geom_max_b;
            this.geom_bounds = geom_bounds;


            // Create the mesh for drawing
            shaded_meshdata = new meshdata_store();
            boundary_lines = new meshdata_store();
            center_circle = new center_circle_store();

            // Add the mesh points
            foreach (var nd_m in temp_nodes.nodeMap)
            {
                node_store nd = nd_m.Value;

                shaded_meshdata.add_mesh_point(nd.node_id, nd.node_pt_x_coord, nd.node_pt_y_coord, nd.node_pt_z_coord, -1);

            }

            // Add the mesh tris
            foreach (var tri_m in temp_tris.elementtriMap)
            {
                elementtri_store tri = tri_m.Value;

                shaded_meshdata.add_mesh_tris(tri.tri_id, tri.nodeid1, tri.nodeid2, tri.nodeid3, tri.material_id);

            }

            // Add the mesh quads
            foreach (var quad_m in temp_quads.elementquadMap)
            {
                elementquad_store quad = quad_m.Value;

                shaded_meshdata.add_mesh_quads(quad.quad_id, quad.nodeid1, quad.nodeid2, quad.nodeid3, quad.nodeid4, quad.material_id);

            }

            // Create the mesh boundary lines
            // Add the boundary points for Unit Circle
            int pt_count = 100;

            for (int i = 0; i <pt_count; i++)
            {
                // Create the points for circle
                double angle = (2.0 * Math.PI * i) / (double)pt_count;
                double x = 0.0 + (1000.0d * Math.Cos(angle));
                double y = 0.0 + (1000.0d * Math.Sin(angle));

                boundary_lines.add_mesh_point(i, x, y, 0.0, -1);

                if(i < pt_count-1)
                {
                    boundary_lines.add_mesh_lines(i, i, i + 1, 2);

                }

            }

            boundary_lines.add_mesh_lines(pt_count - 1, pt_count - 1, 0, 2);


            // Add the boundary points for Rectangle
            int pt_index = pt_count;

            boundary_lines.add_mesh_point(pt_index, -4000.0, -4000.0, 0.0, -1);
            boundary_lines.add_mesh_point(pt_index + 1, -4000.0, 4000.0, 0.0, -1);
            boundary_lines.add_mesh_point(pt_index + 2, 4000.0, 4000.0, 0.0, -1);
            boundary_lines.add_mesh_point(pt_index + 3, 4000.0, -4000.0, 0.0, -1);

            boundary_lines.add_mesh_lines(pt_index, pt_index, pt_index + 1, 2);
            boundary_lines.add_mesh_lines(pt_index + 1, pt_index + 1, pt_index + 2, 2);
            boundary_lines.add_mesh_lines(pt_index + 2, pt_index + 2, pt_index + 3, 2);
            boundary_lines.add_mesh_lines(pt_index + 3, pt_index + 3, pt_index, 2);




            // Set the openTK buffer
            shaded_meshdata.set_shader();
            shaded_meshdata.set_buffer();
            boundary_lines.set_shader();
            boundary_lines.set_buffer();


            // Set the buffer of selection rectangle
            selection_rectangle.set_shader();
            selection_circle.set_shader();

            // Set the buffer of selection rectangle
            selection_rectangle.set_buffer();
            selection_circle.set_buffer();


            // Update the openGL uniform
            shaded_meshdata.update_openTK_uniforms(true, true, true, graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
                0.2f);

            boundary_lines.update_openTK_uniforms(true, true, true, graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);

            center_circle.update_openTK_uniforms(true, true, true, graphic_events_control);

        }

        public void paint_model()
        {
            if (isModelSet == false)
                return;


            // Paint the mesh quad & mesh tris
            if (gvariables_static.is_paint_mesh == true)
            {
                shaded_meshdata.paint_static_mesh();

            }

            gvariables_static.LineWidth = 1.5f;
            boundary_lines.paint_static_mesh_lines();

            // Paint the center circle
            center_circle.paint_center_circle();

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency)
        {
            if (isModelSet == false)
                return;


            shaded_meshdata.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix,
                graphic_events_control.viewMatrix,
                graphic_events_control.geom_transparency);


            boundary_lines.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                graphic_events_control.projectionMatrix,
                graphic_events_control.modelMatrix,
                graphic_events_control.viewMatrix,
                graphic_events_control.geom_transparency);

            center_circle.update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                graphic_events_control);

        }



        public void select_mesh_objects(Vector2 o_pt, Vector2 c_pt, bool isRightButton)
        {
            //// Perform the select option
            //if (isMaterialUpdateInProgress == true)
            //{
            //    // Select the elements for material property update
            //    List<int> selected_tri_ids = meshdata.mesh_tris.is_tri_selected(o_pt, c_pt, graphic_events_control);
            //    List<int> selected_quad_ids = meshdata.is_quad_selected(o_pt, c_pt, graphic_events_control);

            //    meshdata.add_selected_tris(selected_tri_ids, isRightButton);
            //    meshdata.add_selected_quads(selected_quad_ids, isRightButton);

            //}

            //if (isConstraintUpdateInProgress == true || isLoadUpdateInProgress == true)
            //{
            //    // Select the nodes for constraint update
            //    List<int> selected_point_index = meshdata.mesh_points.is_point_selected(o_pt, c_pt, graphic_events_control);

            //    meshdata.add_selected_points(selected_point_index, isRightButton);

            //}

        }



        public void drag_operation_start(Vector2 o_pt)
        {
            // Center circle drag test
            center_circle.circle_drag_start(o_pt, graphic_events_control);

        }



        public void drag_operation_inprogress(Vector2 c_pt)
        {
            // Center circle drag 
            center_circle.circle_drag_inprogress(c_pt, graphic_events_control);

        }


        public void drag_operation_end()
        {
            // Set the drag end
            center_circle.circle_drag_end();

        }



    }
}
