﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal
{
    internal class Conexion
    {
        private MySqlConnection conexion;
        private string server = "localhost";
        private string database = "Envios_FhelCorp";
        private string user = "root";
        private string password = "Francoturro20!";
        private string cadenaConexion;

        public Conexion()
        {
            cadenaConexion = "Database=" + database + 
                             "; DataSource=" + server + 
                             "; User Id= " + user + 
                             "; Password=" + password;

        }

        public MySqlConnection Connection { get; internal set; }

        public MySqlConnection GetConexion()
        {
            if (conexion == null)
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
            }

            return conexion;

        }
    }

}
