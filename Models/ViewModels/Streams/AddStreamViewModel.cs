using System.ComponentModel.DataAnnotations;
using Models.Enums;

namespace Models.ViewModels.Streams
{
    public class AddStreamViewModel
    {
        [Display(Name = "URL of the service")]
        public string Url { get; set; }
        
        [Display(Name = "Name of the Service")]
        public string Name { get; set; }
        
        [Display(Name = "Type of Service to export")]
        public SinkTypeEnum SinkType { get; set; }
    }
}