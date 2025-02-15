using Microsoft.AspNetCore.Mvc;
using AtlanticQiAPI.Models;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly AppDbContext _context;

    // Constructor que recibe el contexto de la base de datos
    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene la lista de todos los clientes registrados en la base de datos.
    /// </summary>
    /// <returns>Lista de clientes.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        return await _context.Clients.ToListAsync();
    }

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    /// <param name="id">ID del cliente.</param>
    /// <returns>El cliente si se encuentra, o un mensaje de error si no existe.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
            return NotFound(new { error = "Client not found" });

        return client;
    }

    /// <summary>
    /// Crea un nuevo cliente en la base de datos.
    /// </summary>
    /// <param name="client">Objeto cliente con los datos a registrar.</param>
    /// <returns>El cliente creado.</returns>
    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(Client client)
    {
        if (client == null)
            return BadRequest(new { error = "Invalid client data" });

        // Validamos que los campos obligatorios no estén vacíos
        if (string.IsNullOrWhiteSpace(client.firstName) || string.IsNullOrWhiteSpace(client.lastName) ||
            string.IsNullOrWhiteSpace(client.documentNumber))
        {
            return BadRequest(new { error = "First name, last name, and document number are required" });
        }

        try
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClient), new { id = client.idClient }, client);
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new { error = "Database error: " + ex.Message });
        }
    }

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    /// <param name="id">ID del cliente a actualizar.</param>
    /// <param name="client">Objeto cliente con los nuevos datos.</param>
    /// <returns>NoContent si la actualización es exitosa, o un mensaje de error en caso de fallo.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, Client client)
    {
        var existingClient = await _context.Clients.FindAsync(id);
        if (existingClient == null)
        {
            return NotFound("Client not found");
        }

        // Copia manualmente las propiedades, excepto el idClient
        existingClient.firstName = client.firstName;
        existingClient.lastName = client.lastName;
        existingClient.email = client.email;
        existingClient.phone = client.phone;
        existingClient.birthDate = client.birthDate;
        existingClient.documentType = client.documentType;
        existingClient.documentNumber = client.documentNumber;
        existingClient.address = client.address;
        existingClient.city = client.city;
        existingClient.updatedAt = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Conflict("Concurrency error: " + ex.Message);
        }

        return NoContent();
    }

    /// <summary>
    /// Elimina un cliente de la base de datos por su ID.
    /// </summary>
    /// <param name="id">ID del cliente a eliminar.</param>
    /// <returns>NoContent si se eliminó correctamente, o un mensaje de error si no se encontró el cliente.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
            return NotFound(new { error = "Client not found" });

        try
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new { error = "Database error: " + ex.Message });
        }
    }
}
