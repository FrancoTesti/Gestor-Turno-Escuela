namespace GTE.DTOs
{
    public class AlumnosPorCursoDTO
    {
        public int IdCurso { get; set; }
        public string Grado { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
