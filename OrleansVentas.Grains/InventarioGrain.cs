using OrleansVentas.Abstractions;

namespace OrleansVentas.Grains;

public class InventarioGrain : Grain, IInventarioGrain<Producto>
{
    private List<Producto> _producto = new List<Producto>();

    public Task<bool> AgregarProducto(int productoId, string nombre, decimal precio, int cantidad)
    {
        if (productoId == 0)
            return Task.FromResult(false);

        _producto.Add(
            new Producto
            {
                ProductoId = productoId,
                Nombre = nombre,
                Precio = precio,
                Cantidad = cantidad
            }
        );
        return Task.FromResult(true);
    }

    public Task<Producto> ConsultarStock(int productoId)
    {
        var producto = _producto.FirstOrDefault(p => p.ProductoId == productoId);
        return Task.FromResult(producto!);
    }

    public Task<bool> ReducirStock(int productoiId, int cantidad)
    {
        // Esto está simulado.
        return Task.FromResult(true);
    }
}
