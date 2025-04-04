using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using OrleansVentas.Abstractions;
using OrleansVentas.Grains;

class Program
{
    static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder()
            .UseOrleansClient(clientBuilder =>
            {
                clientBuilder.UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "dev";
                        options.ServiceId = "OrleansVentas";
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddLogging(); // Agregar logging de forma correcta
                    });
            })
            .Build();

        await host.StartAsync();

        // *************************************************************************
        var client = host.Services.GetRequiredService<IClusterClient>();

        Console.Clear();
        Console.WriteLine("Orleans Ventas\n");

        var inventario = client.GetGrain<IInventarioGrain<Producto>>("global");
        
        // Añadimos algunos productos al inventario.
        await inventario.AgregarProducto(1, "Laptop", 1000, 10);
        await inventario.AgregarProducto(2, "Notebook", 25000, 8);
        await inventario.AgregarProducto(3, "PC Desktop", 18000, 12);
        await inventario.AgregarProducto(4, "Server", 150000, 5);
        await inventario.AgregarProducto(5, "UPS", 4500, 15);

        // Realizamos algunas compras y las agregamos al carrito...      
        var carrito = client.GetGrain<ICarritoVirtualGrain>(Guid.NewGuid());

        // Mostramos el producto para la compra.
        Console.WriteLine("Productos en el carrito:\n");

        // Eligimos un producto.
        int productoId = 0;
        int cantidad = 0;
        decimal total = 0;
        decimal totalCompra = 0;
                
        Console.WriteLine("Detalles de los Productos:\n");
        
        // Elegimos el producto 1. 
        productoId = 1;
        cantidad = 2;
        await carrito.AgregarProducto(productoId, cantidad);
        var producto1 = await inventario.ConsultarStock(productoId);
        total = await carrito.ConfirmarCompra();
        // Procedemos a computar la cantidad para la compra y obtener el precio.
        Console.WriteLine(
            "ID: {0}, Nombre: {1}, Precio: $ {2:f2}",
            producto1.ProductoId, producto1.Nombre, producto1.Precio
        );
        Console.WriteLine("Cantidad: {0} - Total: $ {1:f2}", cantidad, total);
        Console.WriteLine("");
        totalCompra += total;

        // Elegimos el producto 3. 
        productoId = 3;
        cantidad = 4;
        await carrito.AgregarProducto(productoId, cantidad);
        var producto2 = await inventario.ConsultarStock(productoId);
        total = await carrito.ConfirmarCompra();
        // Procedemos a computar la cantidad para la compra y obtener el precio.
        Console.WriteLine(
            "ID: {0}, Nombre: {1}, Precio: $ {2:f2}",
            producto2.ProductoId, producto2.Nombre, producto2.Precio
        );
        Console.WriteLine("Cantidad: {0} - Total: $ {1:f2}", cantidad, total);
        Console.WriteLine("");
        totalCompra += total;

        // Sumarizar el total de compra.
        Console.WriteLine("Total General: $ {0:f2}", totalCompra);
        // *************************************************************************

        await host.WaitForShutdownAsync(); // Mantiene el cliente ejecutándose
    }
}