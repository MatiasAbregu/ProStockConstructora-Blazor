using DTO.DTOs_Usuarios;

namespace ProStockConstructora.Client
{
    public class DatosSesion
    {
        public event Action OnChange;
        private VerUsuarioDTO _Usuario;
        public VerUsuarioDTO Usuario { 
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
