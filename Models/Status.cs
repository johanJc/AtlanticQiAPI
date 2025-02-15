using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AtlanticQiAPI.Models;

[Table("Status")] // Nombre correcto de la tabla
public class Status
{
    public int Id { get; set; }
    public string Name { get; set; }

}