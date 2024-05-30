using ControllerDomain.Entities;

namespace SkudWebApplication.Dto
{
    public class ControllerWithLastLocation
    {
        public int Id { get; set; }
        public string Sn { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // тип контроллера.
        public string FwVer { get; set; } = string.Empty; // версия прошивки контроллера
        public string ComFwVer { get; set; } = string.Empty; // версия прошивки модуля связи
        public string IpAddress { get; set; } = string.Empty; // ip адрес контроллера в локальной сети
        public DateTime LastPing { get; set; } // время последнего принятого ping запроса
        public DateTime LastPowerOn { get; set; } // время последнего принятого включения
    }
}
