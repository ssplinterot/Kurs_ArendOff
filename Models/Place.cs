using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_ArendOff.Models
{
    internal class Place
    {
        [Key]
        public string PlaceIdentifier { get; set; }                                                  
        public string Name { get; set; }// Название магазина/арендатора
        public double Area { get; set; }// Площадь в м²
        public decimal BaseRent { get; set; } // Базовая стоимость аренды (за м²)
       public string Status { get; set; }// Статус: Свободно, Занято, Ремонт
       public string Description { get; set; }// Дополнительное описание

        [NotMapped]
        public string FullStatus => Status == "Занято" ? Name : Status;

        [NotMapped]
        public decimal TotalRent => (decimal)Area * BaseRent;
    }
}
