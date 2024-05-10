﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmericaNews.Data.Models
{
    public class NoticiaModel
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public bool Ocultar { get; set; }
        public int IDUsuario { get; set; }
        public int? ID_ADM_Aprovou { get; set; }
        public DateTime? DataAprovada { get; set; }
    }
}