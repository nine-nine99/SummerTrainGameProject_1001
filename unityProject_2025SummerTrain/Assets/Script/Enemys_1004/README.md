# 2D敌人系统 (2D Enemy System)

这个文件夹包含了2D游戏中敌人系统的核心代码，主要用于测试玩家和玩家族人的攻击。

## 文件结构

- `EnemyBase.cs` - 敌人基类，所有敌人对象的基类
- `TargetDummy.cs` - 靶子敌人类，继承自EnemyBase，用于测试攻击
- `EnemyUsageExample.cs` - 使用示例脚本，展示如何创建和使用敌人对象
- `README.md` - 本说明文件

## 核心功能

### 1. EnemyBase (敌人基类)

**主要特性：**
- 自动设置"Enemy"标签
- 自动添加碰撞体组件（如果没有的话）
- 生命值管理系统
- 伤害处理系统
- 死亡处理系统

**关键方法：**
- `TakeDamage(float damage, GameObject attacker)` - 受到伤害
- `Die()` - 死亡处理
- `ResetEnemy()` - 重置敌人状态
- `GetCurrentHealth()` - 获取当前生命值
- `IsDead()` - 检查是否已死亡

### 2. TargetDummy (靶子敌人)

**主要特性：**
- 继承自EnemyBase的所有功能
- 伤害数字显示
- 击中时的颜色闪烁效果
- 高生命值（1000点）便于测试
- 静态物体，不会移动

**视觉效果：**
- 被击中时显示伤害数字
- 被击中时颜色变为红色
- 死亡时变为灰色

## 使用方法

### 方法1：使用预制体

1. 创建一个GameObject
2. 添加`TargetDummy`组件
3. 设置碰撞体大小和材质
4. 保存为预制体
5. 在`EnemyUsageExample`中引用该预制体

### 方法2：动态创建

```csharp
// 创建靶子敌人
GameObject enemyObject = new GameObject("TargetDummy");
enemyObject.transform.position = new Vector3(0, 0, 0);
TargetDummy targetDummy = enemyObject.AddComponent<TargetDummy>();
```

### 方法3：使用EnemyUsageExample

1. 将`EnemyUsageExample`脚本添加到场景中的物体上
2. 运行游戏
3. 使用键盘控制：
   - `T` - 在鼠标位置生成靶子
   - `R` - 重置所有敌人
   - `A` - 攻击所有敌人

## 标签和碰撞体

### Enemy标签
- 所有敌人对象都会自动设置"Enemy"标签
- 可以通过标签来识别敌人对象

### 碰撞体
- 自动添加BoxCollider2D（如果没有碰撞体）
- 自动添加Rigidbody2D（设置为运动学模式）
- 碰撞体大小：1x2（2D尺寸，可自定义）

## 自定义扩展

### 创建新的敌人类

```csharp
public class MyCustomEnemy : EnemyBase
{
    protected override void OnDamageTaken(float damage, GameObject attacker)
    {
        base.OnDamageTaken(damage, attacker);
        // 添加自定义的受伤效果
    }
    
    protected override void Die()
    {
        // 添加自定义的死亡效果
        base.Die();
    }
}
```

### 修改靶子行为

可以在`TargetDummy`类中修改：
- 伤害数字的显示位置和颜色
- 击中时的颜色效果
- 生命值大小
- 碰撞体大小

## 注意事项

1. **标签设置**：所有敌人对象都会自动设置"Enemy"标签
2. **碰撞体**：如果没有碰撞体，会自动添加BoxCollider
3. **生命值**：靶子敌人默认有1000点生命值，便于测试
4. **物理**：靶子敌人使用运动学Rigidbody，不会受重力影响
5. **性能**：伤害数字会自动销毁，避免内存泄漏

## 调试功能

### 编辑器中的测试功能

在`TargetDummy`组件上右键可以看到：
- "测试伤害 (10点)" - 测试受到10点伤害
- "测试死亡" - 测试死亡效果

在`EnemyUsageExample`组件上右键可以看到：
- "测试攻击所有敌人" - 攻击场景中所有敌人
- "重置所有敌人" - 重置所有敌人状态

## 扩展建议

1. **音效系统**：添加击中音效和死亡音效
2. **粒子效果**：添加击中时的粒子特效
3. **AI系统**：为敌人添加AI行为
4. **动画系统**：添加受伤和死亡动画
5. **网络同步**：支持多人游戏中的敌人同步 