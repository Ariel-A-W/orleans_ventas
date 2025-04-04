using OrleansVentas.Abstractions;

namespace OrleansVentas.Grains;

public class CarritoVirtualGrain : Grain, ICarritoVirtualGrain
{
    private Dictionary<int, int> _productos = new();

    public Task AgregarProducto(int productoId, int cantidad)
    {
        if (_productos.ContainsKey(productoId))
        {
            _productos[productoId] += cantidad;
        }
        else
        {
            _productos[productoId] = cantidad;
        }
        return Task.CompletedTask;
    }

    public async Task<decimal> ConfirmarCompra()
    {
        decimal total = 0;

        foreach (var (productoId, cantidad) in _productos)
        {
            var producto = GrainFactory.GetGrain<IInventarioGrain<Producto>>("global");
            var stock = await producto.ConsultarStock(productoId);

            if (stock == null)
                throw new Exception($"El producto {productoId} no existe.");

            total += stock.Precio * cantidad;
        }

        //_productos.Clear();
        return total;
    }

    public Task<Dictionary<int, int>> ObtenerProductos()
    {
        return Task.FromResult(_productos);
    }

    public Task RemoverProducto(int productoId)
    {
        _productos.Remove(productoId);
        return Task.CompletedTask;
    }
}
