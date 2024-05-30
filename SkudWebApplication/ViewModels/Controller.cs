using ControllerDomain.Entities;

namespace SkudWebApplication.ViewModels
{
    public class Controller
    {
        public int Id { get; set; }
        public string Sn { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // тип контроллера.
        public string FwVer { get; set; } = string.Empty; // версия прошивки контроллера
        public string ComFwVer { get; set; } = string.Empty; // версия прошивки модуля связи
        public string IpAddress { get; set; } = string.Empty; // ip адрес контроллера в локальной сети
        public int? LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public DateTime LastPing { get; set; } // время последнего принятого ping запроса
        public DateTime LastPowerOn { get; set; } // время последнего принятого включения
    }
}