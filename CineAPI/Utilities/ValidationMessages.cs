namespace CineAPI.Utilities;

public static class ValidationMessages
{
    public static readonly string EmptyField = "Debe ingresar {PropertyName}.";
    public static readonly string ValidField = "Debe ingresar {PropertyName} válido.";
    public static readonly string EmptyTitleMovie = "Debe ingresar el título de la película.";
    public static readonly string MaxLength50 = "El {PropertyName} de la película no puede exceder los 50 caracteres.";
    public static readonly string ReleaseDateFormat = "La fecha de lanzamiento debe estar en formato dd/MM/aaaa.";
    public static readonly string EmptyDuration = "Debe ingresar la duración en minutos de la película";
    public static readonly string MaxLengthDescription = "La descripción de la película no puede exceder los 500 caracteres.";
    public static readonly string EmptyMovieId = "Debe ingresar el identificador de la película.";
    public static readonly string MovieNotFound = "No existe una película con ese identificador.";
}
