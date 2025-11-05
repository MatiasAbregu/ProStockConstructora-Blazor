using DTO.DTOs_Usuarios;

namespace ProStockConstructora.Client
{
    public class DatosSesion
    {
        public event Action OnChange;
        private DatosUsuario _Usuario;
        public DatosUsuario Usuario { 
            get => _Usuario; 
            set
            {
                _Usuario = value;
                OnChange.Invoke();
            }
        }

        
        public void LimpiarDatos()
        {
            Usuario = null;
        }
    }
}
