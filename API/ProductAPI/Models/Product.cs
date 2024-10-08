﻿using System.Text.Json.Serialization;

namespace ProductAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }

        // Chave estrangeira para o Departamento
        // Este campo sera ignorado na serialização para JSON
        [JsonIgnore]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        // Este campo sera ignorado na serializaçao para JSON
        [JsonIgnore] 
        public bool IsDeleted { get; set; }


    }
}
