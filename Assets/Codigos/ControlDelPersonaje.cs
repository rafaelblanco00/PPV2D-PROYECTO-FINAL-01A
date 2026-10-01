using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControlDelPersonaje : MonoBehaviour
{
    public Animator _animator;
    public Rigidbody2D _rigidBody2D;
    public CharacterController2D _characterController2D;

    MisAccionesDeEntrada _misAccionesDeEntrada;

    public float _velocidad_de_reproduccion_juego = 1f;

    public bool _agachar = false;
    public bool _saltar = false;
    public bool _per_corriendo = false;

    public float _orientacion = 0;
    public float _velocidad_caminar = 40f;
    public float _velocidad_correr = 100f;
    public float _velocidad = 0f;
    public float _movimiento_horizontal = 0f;

    public bool _botonAtrasPresionado = false;
    public bool _botonAdelantePresionado = false;

    public float _fuerza_rebote = 400f;

    public bool _recibir_herida = false;

    public bool _per_orientacion_derecha_bool = true;
    public float _per_orientacion_derecha_int = 1f;
    public float _per_velocidad_x = 0f;
    public float _per_velocidad_y = 0f;
    public float _per_revivir_espera = 2f;
    public bool _personaje_en_suelo = false;
    public bool _personaje_agachado = false;

    private void Awake()
    {
        _misAccionesDeEntrada = new MisAccionesDeEntrada();

        _misAccionesDeEntrada.MiMapaDeAcciones.CaminarAdelante.performed += Bot_CaminarAdelantePresionado;
        _misAccionesDeEntrada.MiMapaDeAcciones.CaminarAdelante.canceled += Bot_CaminarAdelanteLiberado;

        _misAccionesDeEntrada.MiMapaDeAcciones.CaminarAtras.performed += Bot_CaminarAtrasPresionado;
        _misAccionesDeEntrada.MiMapaDeAcciones.CaminarAtras.canceled += Bot_CaminarAtrasLiberado;

        _misAccionesDeEntrada.MiMapaDeAcciones.Agachar.performed += Bot_AgacharPresionado;
        _misAccionesDeEntrada.MiMapaDeAcciones.Agachar.canceled += Bot_AgacharLiberado;

        _misAccionesDeEntrada.MiMapaDeAcciones.Saltar.performed += Bot_SaltarPresionado;
        _misAccionesDeEntrada.MiMapaDeAcciones.Saltar.canceled += Bot_SaltarLiberado;

        _misAccionesDeEntrada.MiMapaDeAcciones.Correr.performed += Bot_CorrerPresionado;
        _misAccionesDeEntrada.MiMapaDeAcciones.Correr.canceled += Bot_CorrerLiberado;

    }

    private void Bot_CaminarAdelantePresionado(InputAction.CallbackContext obj)
    {
        CaminarAdelanteAtrasPreLib("BotCaminarAdleantePresionado");
    }

    private void Bot_CaminarAdelanteLiberado(InputAction.CallbackContext obj)
    {
        CaminarAdelanteAtrasPreLib("BotCaminarAdleanteLiberado");
    }

    private void Bot_CaminarAtrasPresionado(InputAction.CallbackContext obj)
    {
        CaminarAdelanteAtrasPreLib("BotCaminarAtrasPresionado");
    }

    private void Bot_CaminarAtrasLiberado(InputAction.CallbackContext obj)
    {
        CaminarAdelanteAtrasPreLib("BotCaminarAtrasLiberado");
    }

    private void Bot_AgacharPresionado(InputAction.CallbackContext obj)
    {
        _agachar = true;
    }

    private void Bot_AgacharLiberado(InputAction.CallbackContext obj)
    {
        _agachar = false;
    }

    private void Bot_SaltarPresionado(InputAction.CallbackContext obj)
    {
        if (_personaje_agachado == true)
        {
            _saltar = false;
        }
        else
        {
            _saltar = true;
        }
    }

    private void Bot_SaltarLiberado(InputAction.CallbackContext obj)
    {
        _rigidBody2D.gravityScale = 5;
    }

    private void Bot_CorrerPresionado(InputAction.CallbackContext obj)
    {
        _animator.SetBool("Correr", true);
    }

    private void Bot_CorrerLiberado(InputAction.CallbackContext obj)
    {
        _animator.SetBool("Correr", false);
    }

    private void CaminarAdelanteAtrasPreLib(string caso)
    {
        switch (caso)
        {
            case "BotCaminarAdleantePresionado":
                _botonAdelantePresionado = true;
                if (_botonAtrasPresionado == true)
                {
                    _animator.SetBool("Caminar", false);
                    _animator.SetBool("Ocio", true);
                    _orientacion = 0;
                }
                else if(_botonAtrasPresionado == false)
                {
                    _animator.SetBool("Caminar", true);
                    _animator.SetBool("Ocio", false);
                    _orientacion = 1;
                }
                break;
            case "BotCaminarAdleanteLiberado":
                _botonAdelantePresionado = false;
                if (_botonAtrasPresionado == true)
                {
                    _animator.SetBool("Caminar", true);
                    _animator.SetBool("Ocio", false);
                    _orientacion = -1;
                }
                else if (_botonAtrasPresionado == false)
                {
                    _animator.SetBool("Caminar", false);
                    _animator.SetBool("Ocio", true);
                    _orientacion = 0;
                }
                break;
            case "BotCaminarAtrasPresionado":
                _botonAtrasPresionado = true;
                if (_botonAdelantePresionado == true)
                {
                    _animator.SetBool("Caminar", false);
                    _animator.SetBool("Ocio", true);
                    _orientacion = 0;
                }
                else if (_botonAdelantePresionado == false)
                {
                    _animator.SetBool("Caminar", true);
                    _animator.SetBool("Ocio", false);
                    _orientacion = -1;
                }
                break;
            case "BotCaminarAtrasLiberado":
                _botonAtrasPresionado = false;
                if (_botonAdelantePresionado == true)
                {
                    _animator.SetBool("Caminar", true);
                    _animator.SetBool("Ocio", false);
                    _orientacion = 1;
                }
                else if (_botonAdelantePresionado == false)
                {
                    _animator.SetBool("Caminar", false);
                    _animator.SetBool("Ocio", true);
                    _orientacion = 0;
                }
                break;
        }
    }

    private void Start()
    {
        // ajecutar tareas al iniciar la aplicaci�n, despues del despertar
    }

    private void Update()
    {
        _personaje_en_suelo = _characterController2D.m_Grounded;
        _personaje_agachado = _characterController2D.m_Crouched;
        _per_velocidad_x = _rigidBody2D.linearVelocity.x;
        _per_velocidad_y = _rigidBody2D.linearVelocity.y;
        _per_orientacion_derecha_bool = _characterController2D.m_FacingRight;
        _per_corriendo = _animator.GetBool("Correr");

        _animator.SetBool("EnSuelo",_personaje_en_suelo);
        _animator.SetBool("Agachar",_personaje_agachado);
        _animator.SetFloat("VelocidadX", _per_velocidad_x);
        _animator.SetFloat("VelocidadY", _per_velocidad_y);

        if (_orientacion != 0)
        {
            _animator.SetBool("Ocio", false);
        }

        if (_animator.GetBool("EnSuelo") == true)
        {
            _rigidBody2D.gravityScale = 2;
        }

        if (_per_orientacion_derecha_bool == true)
        {
            _per_orientacion_derecha_int = 1;
        }
        else if(_per_orientacion_derecha_bool == false)
        {
            _per_orientacion_derecha_int = -1;
        }

        Time.timeScale = _velocidad_de_reproduccion_juego;

    }
    private void LateUpdate()
    {
        if (
            _animator.GetBool("Correr") == true && 
            _animator.GetBool("EnSuelo") == true // evaluar la eliminacion de esto
        ){
            _velocidad = _velocidad_correr;
        }
        else
        {
            _velocidad = _velocidad_caminar;
        }

        _movimiento_horizontal = Time.fixedDeltaTime * _velocidad * _orientacion;
        _characterController2D.Move(_movimiento_horizontal,_agachar,_saltar);
        _saltar = false; // implementar el doble salto

        if (_rigidBody2D.linearVelocity.y < 0)
        {
            _rigidBody2D.gravityScale = 5f;
            if (_animator.GetBool("HerirBool") == true)
            {
                _characterController2D.GetComponent<Rigidbody2D>().gravityScale = 10f;
            }
        }

    }
    private void OnEnable()
    {
        _misAccionesDeEntrada.Enable();
    }
    private void OnDisable()
    {
        _misAccionesDeEntrada.Disable();
    }

    [ContextMenu("Herir Personaje")]
    public void HerirPersonaje()
    {
        _misAccionesDeEntrada.MiMapaDeAcciones.Disable();
        if (_recibir_herida == false)
        {
            _recibir_herida = true;
            _animator.SetBool("HerirBool", true);
            if (
                _animator.GetBool("EnSuelo") == true &&
                _animator.GetBool("HerirBool") == true
             ){
                _characterController2D.m_Rigidbody2D.AddForce(new Vector2(0f,_fuerza_rebote));
            }

            if (
                _animator.GetBool("EnSuelo") == false &&
                _animator.GetBool("HerirBool") == true
            ){
                if (_animator.GetFloat("VelocidadY") <= 0f)
                {
                    _characterController2D.m_Rigidbody2D.AddForce(new Vector2(0f, _fuerza_rebote * 2));
                }
            }

            StartCoroutine(RevivirPersonajeCorrutina());

        }
    }

    [ContextMenu("Rebotar Personaje")]
    public void RebotarPersonaje()
    {
        _rigidBody2D.AddForce(new Vector2(0f,_fuerza_rebote));
    }

    public void RevivirPersonaje()
    {
        _animator.SetTrigger("Revivir");
        _animator.SetBool("HerirBool", false);
        _recibir_herida = false;
        _misAccionesDeEntrada.MiMapaDeAcciones.Enable();
    }

    IEnumerator RevivirPersonajeCorrutina()
    {
        yield return new WaitForSeconds(_per_revivir_espera);
        RevivirPersonaje();
    }

}