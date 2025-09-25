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


namespace _2DHelmholtz_solver.src.model_store.fe_objects
{


    public class fedata_store
    {

        public meshdata_store shaded_meshdata;
        bool isModelLoadSuccess = false;



        public fedata_store()
        {
            // (Re)Initialize the data
            shaded_meshdata = new meshdata_store(new Vector3(-1), new Vector3(1), new Vector3(2));

        }

        public void importMesh()
        {
            List<Vector3> nodePtsList = new List<Vector3>();
            isModelLoadSuccess = false;

            // Boundary of the circles
            float circle_size = 1000.0f;

            nodePtsList.Add(new Vector3(-circle_size, -circle_size, 0.0f));
            nodePtsList.Add(new Vector3(-circle_size, circle_size, 0.0f));
            nodePtsList.Add(new Vector3(circle_size, circle_size, 0.0f));
            nodePtsList.Add(new Vector3(circle_size, -circle_size, 0.0f));

            node_list_store temp_nodes = new node_list_store();
            elementtri_list_store temp_tris = new elementtri_list_store();
            elementquad_list_store temp_quads = new elementquad_list_store();


            file_events.import_mesh(ref temp_nodes, ref temp_tris, ref temp_quads, ref isModelLoadSuccess);


            if (isModelLoadSuccess == false)
                return;

            // Set the mesh boundaries
            Vector3 geometry_center = gvariables_static.FindGeometricCenter(nodePtsList);
            Tuple<Vector3, Vector3> geom_extremes = gvariables_static.FindMinMaxXY(nodePtsList);

            Vector3 geom_min_b = geom_extremes.Item1; // Minimum bound
            Vector3 geom_max_b = geom_extremes.Item2; // Maximum bound

            Vector3 geom_bounds = geom_max_b - geom_min_b;


            // Create the mesh for drawing
            shaded_meshdata = new meshdata_store(geom_min_b,geom_max_b, geom_bounds);

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

                shaded_meshdata.add_mesh_quads(quad.quad_id, quad.nodeid1, quad.nodeid2 , quad.nodeid3, quad.nodeid4, quad.material_id);

            }

            // Create the mesh boundaries
            // meshdata.set_mesh_wireframe();


            // Model is set
            shaded_meshdata.is_ModelSet = true;

            // Set the openTK buffer
            shaded_meshdata.set_shader();
            shaded_meshdata.set_buffer();

            // Update the openGL uniform
            shaded_meshdata.update_openTK_uniforms(true, true, true);

        }

        public void paint_model()
        {
            if (isModelLoadSuccess == false)
                return;


            // Paint the mesh quad & mesh tris
            if (gvariables_static.is_paint_mesh == true)
            {
                shaded_meshdata.paint_static_mesh();

            }


        }







    }
}
