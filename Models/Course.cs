
namespace BaltaDataAccess.Models;
public class Course
{
    public Guid Id { get; set; }
    public string Tag { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Url { get; set; }
    public short Level { get; set; }
    public int DurationInMinutes { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool Active { get; set; }
    public bool Featured { get; set; }
    public string Tags { get; set; }
}