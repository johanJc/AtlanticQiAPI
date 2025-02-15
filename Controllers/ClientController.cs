using Microsoft.AspNetCore.Mvc;
using AtlanticQiAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Clients_Email"))
            {
                return BadRequest(new { error = "The email provided is already in use." });
            }
            return BadRequest(new { error = "Database error: " + (ex.InnerException?.Message ?? ex.Message) });
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
    /// Actualiza solo el estado de un cliente.
    /// </summary>
    /// <param name="id">ID del cliente.</param>
    /// <param name="statusId">Nuevo estado.</param>
    /// <returns>NoContent si se actualiza correctamente, o error si no se encuentra el cliente.</returns>
    [HttpPut("{id}/ChangeStatus")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] JsonElement payload)
    {
        // 1️⃣ Validar si se recibe el campo `statusId`
        if (!payload.TryGetProperty("statusId", out JsonElement statusIdElement) ||
            !statusIdElement.TryGetInt32(out int statusId))
        {
            return BadRequest(new { error = "The field 'statusId' is required and must be an integer." });
        }

        // 2️⃣ Verificar si el cliente existe
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound(new { error = "Client not found" });
        }

        // 3️⃣ Actualizar el estado
        client.StatusId = statusId;
        client.updatedAt = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Client status updated successfully" });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new { error = "Database error: " + ex.Message });
        }
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
