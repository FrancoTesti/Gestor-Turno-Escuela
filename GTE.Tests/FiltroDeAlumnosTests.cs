using GTE.Clients;
using GTE.DTOs;

namespace GTE.Tests;

public class FiltroDeAlumnosTests
{
    [Fact]
    public void Un_filtro_vacio_no_envia_ninguna_condicion()
    {
        var filtro = new FiltroDeAlumnos();

        AlumnoCriteriaDTO criterios = filtro.ToCriteria();

        Assert.Null(criterios.Nombre);
        Assert.Null(criterios.Grado);
        Assert.Null(criterios.Curso);
        Assert.Null(criterios.Estado);
    }

    [Fact]
    public void Los_espacios_en_blanco_cuentan_como_filtro_vacio()
    {
        var filtro = new FiltroDeAlumnos
        {
            Nombre = "   ",
            Grado = "",
            Curso = "\t",
            Estado = ""
        };

        AlumnoCriteriaDTO criterios = filtro.ToCriteria();

        Assert.Null(criterios.Nombre);
        Assert.Null(criterios.Grado);
        Assert.Null(criterios.Curso);
        Assert.Null(criterios.Estado);
    }

    [Fact]
    public void Los_valores_se_envian_sin_espacios_sobrantes()
    {
        var filtro = new FiltroDeAlumnos
        {
            Nombre = "  Perez  ",
            Grado = " 3° ",
            Curso = " A ",
            Estado = " Presente "
        };

        AlumnoCriteriaDTO criterios = filtro.ToCriteria();

        Assert.Equal("Perez", criterios.Nombre);
        Assert.Equal("3°", criterios.Grado);
        Assert.Equal("A", criterios.Curso);
        Assert.Equal("Presente", criterios.Estado);
    }

    [Fact]
    public void Un_filtro_con_datos_no_esta_vacio()
    {
        var filtro = new FiltroDeAlumnos { Nombre = "Perez" };

        Assert.False(filtro.EstaVacio());
    }

    [Fact]
    public void Un_filtro_sin_datos_esta_vacio()
    {
        var filtro = new FiltroDeAlumnos { Nombre = "  " };

        Assert.True(filtro.EstaVacio());
    }

    [Fact]
    public void Limpiar_deja_el_filtro_sin_datos()
    {
        var filtro = new FiltroDeAlumnos
        {
            Nombre = "Perez",
            Grado = "3°",
            Curso = "A",
            Estado = "Presente"
        };

        filtro.Limpiar();

        Assert.True(filtro.EstaVacio());
    }
}
