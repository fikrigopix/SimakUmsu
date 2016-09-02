using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIA_Universitas.Models
{
    [MetadataType(typeof(Mstr_RoomAnnotation))]
    public partial class Mstr_Room
    {
    }

    internal sealed class Mstr_RoomAnnotation
    {
        [Required]
        public short Room_Id { get; set; }

        [Required(ErrorMessage = "Kode Ruang Harus diisi")]
        [Display(Name = "Kode Ruang")]
        [Remote("IsRoomCodeExists", "Room", ErrorMessage = "Kode Ruang ini telah ada.")]
        public string Room_Code { get; set; }

        [Display(Name = "Gedung")]
        public Nullable<short> Building_Id { get; set; }

        [Display(Name = "Nama Ruang")]
        public string Room_Name { get; set; }

        [Display(Name = "Deskripsi")]
        public string Description { get; set; }

        [Display(Name = "Kapasitas")]
        public Nullable<int> Capacity { get; set; }

        [Display(Name = "Nama Singkatan")]
        public string Acronym { get; set; }

        [Display(Name = "Status")]
        public bool Is_Active { get; set; }
        
        public string Created_By { get; set; }
        
        public Nullable<System.DateTime> Created_Date { get; set; }
        
        public string Modified_By { get; set; }
        
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}