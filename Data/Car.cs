using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public class Car: BaseEntity
    {
        [Column("Mileage")]
        public uint Mileage { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Desc")]
        public string Desc { get; set; }

        [Column("Image")]
        public string Image { get; set; }

        [Column("GOD")]
        public ushort Year { get; set; }

        [Column("Transmission")]
        public ushort Transmission { get; set; }

        [Column("Fuel")]
        public ushort Fuel { get; set; }

        [Column("Body")]
        public ushort Body { get; set; }

        [Column("Drive")]
        public ushort Drive { get; set; }

        [Column("EngineVolume")]
        public double EngineVolume { get; set; }
    }
}
