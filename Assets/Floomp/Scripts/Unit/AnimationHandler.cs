using Suriyun;
using System.Collections.Generic;
using UnityEngine;


// The third party assets I am using for units came with its own Animation Controller class.
// However, it uses the value of a single int animationIndex variable to set the value.
// My animation handler will bridge the gap between units and this controller by creating a mapping and allowing the
// animations to be set via strings instead
public static class AnimationHandler
{
    private static Dictionary<string, int> animationMappings = new Dictionary<string, int> {
        { StringLibrary.idleAnimation, 1 },
        { StringLibrary.moveAnimation, 2 },
        { StringLibrary.attackAnimation, 3 },
        { StringLibrary.damageAnimation, 4 },
        { StringLibrary.deathAnimation, 5 },
    };

    public static void MoveToAnimation(AnimatorController _controller, string _animation, bool loop = true) {
        if (animationMappings.TryGetValue(_animation, out var animationIndex)) {
            _controller.SetInt(StringLibrary.animationRef + animationIndex, loop);
        }
        else {
            Debug.LogError($"No animation found for controller {_controller} with reference {_animation}");
        }
    }
}
