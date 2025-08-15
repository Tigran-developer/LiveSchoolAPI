using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace WebAPI.Models.Entities
{
    public class ClassPupil
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClassId { get; set; }

        [Required]
        public Guid PupilId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public bool? IsApproved { get; set; }

        [ForeignKey(nameof(PupilId))]
        public Pupil? Pupil { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class? Class { get; set; }
    }

}
