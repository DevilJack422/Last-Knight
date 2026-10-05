using UnityEngine;

public interface IState
{
    void Enter(); //chay 1 lan khi vua vao state
    void Tick(); // chay moi frame khi o state
    void Exit(); // chay 1 lan khi roi state
}