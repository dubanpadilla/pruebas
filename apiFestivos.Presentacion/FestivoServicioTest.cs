using Xunit;
using Moq;
using apiFestivos.Aplicacion.Servicios;
using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Core.Interfaces.Servicios;
using apiFestivos.Dominio.Entidades;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

public class FestivoServicioTest
{
    private readonly Mock<IFestivoRepositorio> _repositorioMock;
    private readonly IFestivoServicio _servicio;

    public FestivoServicioTest()
    {
        _repositorioMock = new Mock<IFestivoRepositorio>();
        _servicio = new FestivoServicio(_repositorioMock.Object);  // Asumiendo que 'FestivoServicio' implementa 'IFestivoServicio'
    }

    [Fact]
    public async Task EsFestivo_FechaFestiva_RetornaTrue()
    {
        // Arrange (Preparamos los datos)
        var idFestivo = 1;
        var fechaFestiva = new DateTime(2024, 12, 25);
        var festivosDelAño = new List<Festivo>
        {
            new Festivo
            {
                Id = idFestivo,
                Nombre = "Navidad",
                Dia = 25,
                Mes = 12,
                IdTipo = 1,
                DiasPascua = 0
            }
        };

        // Configuramos el mock para que devuelva la lista de festivos
        _repositorioMock.Setup(repositorio => repositorio.ObtenerTodos()).ReturnsAsync(festivosDelAño);

        // Act (Ejecutamos el método que estamos probando)
        var resultado = await _servicio.EsFestivo(fechaFestiva);

        // Assert (Verificamos que el resultado sea correcto)
        Assert.True(resultado);
    }

    [Fact]
    public async Task ObtenerFecha_Tipo_RetornaCorrecto()
    {
        // Arrange (Preparamos los datos)
        var idFestivo2 = 5;
        var fechaFestiva2 = new DateTime(2024, 1, 1);
        var festivosDelAño2 = new List<Festivo>
        {
            new Festivo
            {
                Id = idFestivo2,
                Nombre = "Año Nuevo",
                Dia = 1,
                Mes = 1,
                IdTipo = 1,
                DiasPascua = 0
            }
        };

        // Configuramos el mock para que devuelva la lista de festivos del Año Nuevo
        _repositorioMock.Setup(repositorio => repositorio.ObtenerTodos()).ReturnsAsync(festivosDelAño2);

        // Act (Ejecutamos el método que estamos probando)
        var resultado = await _servicio.ObtenerAño(fechaFestiva2.Year);

        // Assert (Verificamos que el resultado sea el esperado)
        Assert.Single(resultado);  // Verifica que solo haya un festivo en el resultado
        Assert.Equal(festivosDelAño2[0].Nombre, resultado[0].Nombre);  // Verifica el nombre del festivo
        Assert.Equal(fechaFestiva2, resultado[0].Fecha);  // Verifica que la fecha sea la correcta
    }
}
