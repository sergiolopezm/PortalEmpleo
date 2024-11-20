namespace PortalEmpleo.WebApp.Models.ViewModels.JobOffers
{
    public class JobOfferListViewModel
    {
        public int Pagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public List<JobOfferViewModel> Ofertas { get; set; }
        public string? Filtro { get; set; }
    }

    public class JobOfferViewModel
    {
        public int IdOferta { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public decimal? Salario { get; set; }
        public string TipoContrato { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string NombreReclutador { get; set; }
        public string Estado { get; set; }
        public string IdReclutador { get; set; }
    }
}
