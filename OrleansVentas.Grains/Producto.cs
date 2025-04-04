namespace OrleansVentas.Grains;

[GenerateSerializer]
public class Producto
{
    [Id(0)] 
    public int ProductoId { get; set; }
    
    [Id(1)] 
    public string? Nombre { get; set; }
    
    [Id(2)] 
    public decimal Precio { get; set; }
    
    [Id(3)] 
    public int Cantidad { get; set; }
}
