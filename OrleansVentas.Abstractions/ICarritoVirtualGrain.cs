using Orleans;

namespace OrleansVentas.Abstractions;

public interface ICarritoVirtualGrain : IGrainWithGuidKey
{
    Task AgregarProducto(int productoId, int cantidad);
    Task RemoverProducto(int productoId); 
    Task<Dictionary<int, int>> ObtenerProductos();
    Task<decimal> ConfirmarCompra();
}
