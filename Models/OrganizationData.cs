using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Kurs_ArendOff.Models;

namespace Kurs_ArendOff.Models
{
    // Модель для хранения данных об арендаторе и договоре
    internal class OrganizationData
    {
        [Key]
        public int Id { get; set; }
        public string OrganizationName { get; set; }// Информация об арендаторе
        public string INN { get; set; } // ИНН или УНП для идентификации

        // Информация о договоре аренды
        public string ContractNumber { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }

        // Информация об арендуемом месте 
        public string PlaceIdentifier { get; set; }

        // Платежи и суммы
        public decimal RentalAmount { get; set; } // Ежемесячная сумма аренды
        public bool IsPaid { get; set; } // Флаг, оплачена ли аренда за текущий период
        public bool IsPinned { get; set; }
    }
}

