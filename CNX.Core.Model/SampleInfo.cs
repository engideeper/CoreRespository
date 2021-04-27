using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNX.Core.Model
{
    public class SampleInfo
    {
        [Key]
        /// <summary>
        /// ID
        /// </summary>
        /// 
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Valid { get; set; }


    }
}
