using Orleans;

namespace OrleansVentas.Abstractions;

public interface IInventarioGrain<T> : IGrainWithStringKey
    where T : class

{
    Task<bool> AgregarProducto(int productoId, string nombre, decimal precio, int cantidad);
    Task<T> ConsultarStock(int productoId);
    Task<bool> ReducirStock(int productoiId, int cantidad);
}
