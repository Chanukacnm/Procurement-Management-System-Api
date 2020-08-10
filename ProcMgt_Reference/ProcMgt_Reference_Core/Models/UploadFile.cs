using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class UploadFile
    {
        public UploadFile()
        {
            Model = new HashSet<Model>();
        }

        [Column("UploadFileID")]
        public Guid UploadFileId { get; set; }
        [Required]
        [StringLength(150)]
        public string UploadFileName { get; set; }
        [Required]
        public string UploadFilePath { get; set; }
        [Required]
        [StringLength(30)]
        public string FileExtension { get; set; }
        public bool IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTime { get; set; }

        [InverseProperty("UploadFile")]
        public virtual ICollection<Model> Model { get; set; }
    }
}
