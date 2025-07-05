public enum State{
    Idle,
    Patrol,
    Escape,
    DashEscape,
    TeleportBack,
    Stun,
    Chase,
    ChaseCoin,// 追逐金币
    Walk,
    Run,
    Jump,
    Attack,
    AttackTarget,// 敌人向目标点逼近 
    CollectCoin,
    FallDown,
    Dead,
    Stop // 停止状态：停止所有行动、动画、速度，停止状态机中的所有协程
}