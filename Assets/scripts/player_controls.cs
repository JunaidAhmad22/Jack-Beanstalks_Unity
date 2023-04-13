using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class player_controls : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInput pl_input;
    CharacterController ch_co;
    Animator anim;

    Vector2 curr_movement_input;
    Vector3 curr_movement;
    bool is_mov_pressed;
    bool is_attack_pressed;


    float rotation_factor=15.0f;


void OnAttack(InputAction.CallbackContext context)
    {
            is_attack_pressed=context.ReadValueAsButton();


    }

    void Awake()
    {
        pl_input=new PlayerInput();
        ch_co = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        pl_input.character_movements.move.started+=context => {

           curr_movement_input=context.ReadValue<Vector2>();
           curr_movement.x=curr_movement_input.x;
           curr_movement.z=curr_movement_input.y;

           is_mov_pressed=curr_movement_input.x!=0  || curr_movement_input.y!=0;
           
            
            };

             pl_input.character_movements.move.canceled+=context => {

           curr_movement_input=context.ReadValue<Vector2>();
           curr_movement.x=curr_movement_input.x;
           curr_movement.z=curr_movement_input.y;

           is_mov_pressed=curr_movement_input.x!=0  || curr_movement_input.y!=0;
            
            };
            pl_input.character_movements.attack.started+=OnAttack;

            pl_input.character_movements.attack.canceled+=OnAttack;
            

    }


    void handleAnimations()
    {
        bool iswalking=anim.GetBool("iswalking");
        bool isattacking=anim.GetBool("isattacking");
      
        
        if(is_mov_pressed && !iswalking)
        {
            anim.SetBool("iswalking",true);
        }

        else if(!is_mov_pressed && iswalking)
        {
            anim.SetBool("iswalking",false);
        }




    }

    void handleRotation()
    {


        Vector3 position_to_look_at;
        

        position_to_look_at.x=curr_movement.x;
        position_to_look_at.y=0.0f;
        position_to_look_at.z=curr_movement.z;
        
        Quaternion curr_rotation=transform.rotation;

    if(is_mov_pressed)
    {
         Quaternion target_rotation=Quaternion.LookRotation(position_to_look_at);
        transform.rotation=Quaternion.Slerp(curr_rotation,target_rotation,rotation_factor * Time.deltaTime);

    }
       
    }


    

    // Update is called once per frame
    void Update()
    {   

        if(is_attack_pressed)
             anim.SetBool("isattacking",true);

        else if(!is_attack_pressed)
             anim.SetBool("isattacking",false);     

                handleRotation();
              handleAnimations();
              ch_co.Move(curr_movement * Time.deltaTime);
        
    }

    void OnEnable()
    {
        pl_input.character_movements.Enable();


    }
     void OnDisable()
    {
        pl_input.character_movements.Disable();
        

    }

}
