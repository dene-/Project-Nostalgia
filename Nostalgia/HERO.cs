using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Xft;

public class HERO : Photon.MonoBehaviour
{
    private HERO_STATE _state;
    private bool almostSingleHook;
    private string attackAnimation;
    private int attackLoop;
    private bool attackMove;
    private bool attackReleased;
    public AudioSource audio_ally;
    public AudioSource audio_hitwall;
    private GameObject badGuy;
    private bool bigLean;
    private float buffTime;
    public GameObject bulletLeft;
    private int bulletMAX = 7;
    public GameObject bulletRight;
    private bool buttonAttackRelease;
    public bool canJump = true;
    public GameObject checkBoxLeft;
    public GameObject checkBoxRight;
    public string currentAnimation;
    private int currentBladeNum = 5;
    private float currentBladeSta = 100f;
    private BUFF currentBuff;
    public Camera currentCamera;
    private float currentGas = 100f;
    public float currentSpeed;
    private bool dashD;
    private Vector3 dashDirection;
    private bool dashL;
    private bool dashR;
    private float dashTime;
    private bool dashU;
    private Vector3 dashV;
    private float dTapTime = -1f;
    private bool EHold;
    private GameObject eren_titan;
    private int escapeTimes = 1;
    private float facingDirection;
    private float flare1CD;
    private float flare2CD;
    private float flare3CD;
    private float flareTotalCD = 30f;
    private Transform forearmL;
    private Transform forearmR;
    private float gravity = 20f;
    private bool grounded;
    private GameObject gunDummy;
    private Vector3 gunTarget;
    private Transform handL;
    private Transform handR;
    private bool hasDied;
    private bool hookBySomeOne = true;
    public GameObject hookRefL1;
    public GameObject hookRefL2;
    public GameObject hookRefR1;
    public GameObject hookRefR2;
    private bool hookSomeOne;
    private GameObject hookTarget;
    public FengCustomInputs inputManager;
    private float invincible = 3f;
    private bool isLaunchLeft;
    private bool isLaunchRight;
    private bool isLeftHandHooked;
    private bool isMounted;
    private bool isRightHandHooked;
    public float jumpHeight = 2f;
    private bool justGrounded;
    public Transform lastHook;
    private float launchElapsedTimeL;
    private float launchElapsedTimeR;
    private Vector3 launchForce;
    private Vector3 launchPointLeft;
    private Vector3 launchPointRight;
    private bool leanLeft;
    private bool leftArmAim;
    public XWeaponTrail leftbladetrail;
    public XWeaponTrail leftbladetrail2;
    private int leftBulletLeft = 7;
    private bool leftGunHasBullet = true;
    private float lTapTime = -1f;
    public float maxVelocityChange = 10f;
    public AudioSource meatDie;
    public GROUP myGroup;
    private GameObject myHorse;
    private GameObject myNetWorkName;
    public float myScale = 1f;
    public int myTeam = 1;
    private bool needLean;
    private Quaternion oldHeadRotation;
    private float originVM;
    private bool QHold;
    private string reloadAnimation = string.Empty;
    private bool rightArmAim;
    public XWeaponTrail rightbladetrail;
    public XWeaponTrail rightbladetrail2;
    private int rightBulletLeft = 7;
    private bool rightGunHasBullet = true;
    public AudioSource rope;
    private float rTapTime = -1f;
    public HERO_SETUP setup;
    private GameObject skillCD;
    public float skillCDDuration;
    public float skillCDLast;
    private string skillId;
    public AudioSource slash;
    public AudioSource slashHit;
    private ParticleSystem smoke_3dmg;
    private ParticleSystem sparks;
    public float speed = 10f;
    public GameObject speedFX;
    public GameObject speedFX1;
    private ParticleSystem speedFXPS;
    private bool spinning;
    private string standAnimation = "stand";
    private Quaternion targetHeadRotation;
    private Quaternion targetRotation;
    private bool throwedBlades;
    public bool titanForm;
    private GameObject titanWhoGrabMe;
    private int titanWhoGrabMeID;
    private int totalBladeNum = 5;
    public float totalBladeSta = 100f;
    public float totalGas = 100f;
    private Transform upperarmL;
    private Transform upperarmR;
    private float useGasSpeed = 0.2f;
    public bool useGun;
    private float uTapTime = -1f;
    private bool wallJump;
    private float wallRunTime;

    private void applyForceToBody(GameObject GO, Vector3 v)
    {
        GO.rigidbody.AddForce(v);
        GO.rigidbody.AddTorque(UnityEngine.Random.Range((float) -10f, (float) 10f), UnityEngine.Random.Range((float) -10f, (float) 10f), UnityEngine.Random.Range((float) -10f, (float) 10f));
    }

    public void attackAccordingToMouse()
    {
        if (Input.mousePosition.x < (Screen.width * 0.5))
        {
            this.attackAnimation = "attack2";
        }
        else
        {
            this.attackAnimation = "attack1";
        }
    }

    public void attackAccordingToTarget(Transform a)
    {
        Vector3 vector = a.position - base.transform.position;
        float current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        float f = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
        if (((Mathf.Abs(f) < 90f) && (vector.magnitude < 6f)) && ((a.position.y <= (base.transform.position.y + 2f)) && (a.position.y >= (base.transform.position.y - 5f))))
        {
            this.attackAnimation = "attack4";
        }
        else if (f > 0f)
        {
            this.attackAnimation = "attack1";
        }
        else
        {
            this.attackAnimation = "attack2";
        }
    }

    private void Awake()
    {
        this.setup = base.gameObject.GetComponent<HERO_SETUP>();
        base.rigidbody.freezeRotation = true;
        base.rigidbody.useGravity = false;
        this.handL = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        this.handR = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        this.forearmL = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
        this.forearmR = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        this.upperarmL = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
        this.upperarmR = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
    }

    public void backToHuman()
    {
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        base.rigidbody.velocity = Vector3.zero;
        this.titanForm = false;
        this.ungrabbed();
        this.falseAttack();
        this.skillCDDuration = this.skillCDLast;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(base.gameObject, true, false);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            base.photonView.RPC("backToHumanRPC", PhotonTargets.Others, new object[0]);
        }
    }

    [RPC]
    private void backToHumanRPC()
    {
        this.titanForm = false;
        this.eren_titan = null;
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
    }

    [RPC]
    public void badGuyReleaseMe()
    {
        this.hookBySomeOne = false;
        this.badGuy = null;
    }

    [RPC]
    public void blowAway(Vector3 force)
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            base.rigidbody.AddForce(force, ForceMode.Impulse);
            base.transform.LookAt(base.transform.position);
        }
    }

    private void bodyLean()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            float z = 0f;
            this.needLean = false;
            if ((!this.useGun && (this.state == HERO_STATE.Attack)) && ((this.attackAnimation != "attack3_1") && (this.attackAnimation != "attack3_2")))
            {
                float y = base.rigidbody.velocity.y;
                float x = base.rigidbody.velocity.x;
                float num4 = base.rigidbody.velocity.z;
                float num5 = Mathf.Sqrt((x * x) + (num4 * num4));
                float num6 = Mathf.Atan2(y, num5) * 57.29578f;
                this.targetRotation = Quaternion.Euler(-num6 * (1f - (Vector3.Angle(base.rigidbody.velocity, base.transform.forward) / 90f)), this.facingDirection, 0f);
                if ((this.isLeftHandHooked && (this.bulletLeft != null)) || (this.isRightHandHooked && (this.bulletRight != null)))
                {
                    base.transform.rotation = this.targetRotation;
                }
            }
            else
            {
                if ((this.isLeftHandHooked && (this.bulletLeft != null)) && (this.isRightHandHooked && (this.bulletRight != null)))
                {
                    if (this.almostSingleHook)
                    {
                        this.needLean = true;
                        z = this.getLeanAngle(this.bulletRight.transform.position, true);
                    }
                }
                else if (this.isLeftHandHooked && (this.bulletLeft != null))
                {
                    this.needLean = true;
                    z = this.getLeanAngle(this.bulletLeft.transform.position, true);
                }
                else if (this.isRightHandHooked && (this.bulletRight != null))
                {
                    this.needLean = true;
                    z = this.getLeanAngle(this.bulletRight.transform.position, false);
                }
                if (this.needLean)
                {
                    float a = 0f;
                    if (!this.useGun && (this.state != HERO_STATE.Attack))
                    {
                        a = this.currentSpeed * 0.1f;
                        a = Mathf.Min(a, 20f);
                    }
                    this.targetRotation = Quaternion.Euler(-a, this.facingDirection, z);
                }
                else if (this.state != HERO_STATE.Attack)
                {
                    this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                }
            }
        }
    }

    private void breakApart(Vector3 v, bool isBite)
    {
        GameObject obj6;
        GameObject obj7;
        GameObject obj8;
        GameObject obj9;
        GameObject obj10;
        GameObject obj5 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
        obj5.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
        obj5.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            GameObject gO = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            gO.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
            obj3.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
            obj4.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
            gO.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            obj3.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            obj4.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            this.applyForceToBody(gO, v);
            this.applyForceToBody(obj3, v);
            this.applyForceToBody(obj4, v);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gO, false, false);
            }
        }
        else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj5, false, false);
        }
        this.applyForceToBody(obj5, v);
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
        if (this.useGun)
        {
            obj6 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
            obj7 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
            obj8 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_2"), base.transform.position, base.transform.rotation);
            obj9 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), base.transform.position, base.transform.rotation);
            obj10 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), base.transform.position, base.transform.rotation);
        }
        else
        {
            obj6 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            obj7 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
            obj8 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), base.transform.position, base.transform.rotation);
            obj9 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), base.transform.position, base.transform.rotation);
            obj10 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), base.transform.position, base.transform.rotation);
        }
        obj6.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj7.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj8.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj9.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj10.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        this.applyForceToBody(obj6, v);
        this.applyForceToBody(obj7, v);
        this.applyForceToBody(obj8, v);
        this.applyForceToBody(obj9, v);
        this.applyForceToBody(obj10, v);
    }

    private void bufferUpdate()
    {
        if (this.buffTime > 0f)
        {
            this.buffTime -= Time.deltaTime;
            if (this.buffTime <= 0f)
            {
                this.buffTime = 0f;
                if ((this.currentBuff == BUFF.SpeedUp) && base.animation.IsPlaying("run_sasha"))
                {
                    this.crossFade("run", 0.1f);
                }
                this.currentBuff = BUFF.NoBuff;
            }
        }
    }

    private void calcFlareCD()
    {
        if (this.flare1CD > 0f)
        {
            this.flare1CD -= Time.deltaTime;
            if (this.flare1CD < 0f)
            {
                this.flare1CD = 0f;
            }
        }
        if (this.flare2CD > 0f)
        {
            this.flare2CD -= Time.deltaTime;
            if (this.flare2CD < 0f)
            {
                this.flare2CD = 0f;
            }
        }
        if (this.flare3CD > 0f)
        {
            this.flare3CD -= Time.deltaTime;
            if (this.flare3CD < 0f)
            {
                this.flare3CD = 0f;
            }
        }
    }

    private void calcSkillCD()
    {
        if (this.skillCDDuration > 0f)
        {
            this.skillCDDuration -= Time.deltaTime;
            if (this.skillCDDuration < 0f)
            {
                this.skillCDDuration = 0f;
            }
        }
    }

    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt((2f * this.jumpHeight) * this.gravity);
    }

    private void changeBlade()
    {
        if ((!this.useGun || this.grounded) || (LevelInfo.getInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS))
        {
            this.state = HERO_STATE.ChangeBlade;
            this.throwedBlades = false;
            if (this.useGun)
            {
                if (!this.leftGunHasBullet && !this.rightGunHasBullet)
                {
                    if (this.grounded)
                    {
                        this.reloadAnimation = "AHSS_gun_reload_both";
                    }
                    else
                    {
                        this.reloadAnimation = "AHSS_gun_reload_both_air";
                    }
                }
                else if (!this.leftGunHasBullet)
                {
                    if (this.grounded)
                    {
                        this.reloadAnimation = "AHSS_gun_reload_l";
                    }
                    else
                    {
                        this.reloadAnimation = "AHSS_gun_reload_l_air";
                    }
                }
                else if (!this.rightGunHasBullet)
                {
                    if (this.grounded)
                    {
                        this.reloadAnimation = "AHSS_gun_reload_r";
                    }
                    else
                    {
                        this.reloadAnimation = "AHSS_gun_reload_r_air";
                    }
                }
                else
                {
                    if (this.grounded)
                    {
                        this.reloadAnimation = "AHSS_gun_reload_both";
                    }
                    else
                    {
                        this.reloadAnimation = "AHSS_gun_reload_both_air";
                    }
                    this.leftGunHasBullet = this.rightGunHasBullet = false;
                }
                this.crossFade(this.reloadAnimation, 0.05f);
            }
            else
            {
                if (!this.grounded)
                {
                    this.reloadAnimation = "changeBlade_air";
                }
                else
                {
                    this.reloadAnimation = "changeBlade";
                }
                this.crossFade(this.reloadAnimation, 0.1f);
            }
        }
    }

    private void checkDashDoubleTap()
    {
        if (this.uTapTime >= 0f)
        {
            this.uTapTime += Time.deltaTime;
            if (this.uTapTime > 0.2f)
            {
                this.uTapTime = -1f;
            }
        }
        if (this.dTapTime >= 0f)
        {
            this.dTapTime += Time.deltaTime;
            if (this.dTapTime > 0.2f)
            {
                this.dTapTime = -1f;
            }
        }
        if (this.lTapTime >= 0f)
        {
            this.lTapTime += Time.deltaTime;
            if (this.lTapTime > 0.2f)
            {
                this.lTapTime = -1f;
            }
        }
        if (this.rTapTime >= 0f)
        {
            this.rTapTime += Time.deltaTime;
            if (this.rTapTime > 0.2f)
            {
                this.rTapTime = -1f;
            }
        }
        if (this.inputManager.isInputDown[InputCode.up])
        {
            if (this.uTapTime == -1f)
            {
                this.uTapTime = 0f;
            }
            if (this.uTapTime != 0f)
            {
                this.dashU = true;
            }
        }
        if (this.inputManager.isInputDown[InputCode.down])
        {
            if (this.dTapTime == -1f)
            {
                this.dTapTime = 0f;
            }
            if (this.dTapTime != 0f)
            {
                this.dashD = true;
            }
        }
        if (this.inputManager.isInputDown[InputCode.left])
        {
            if (this.lTapTime == -1f)
            {
                this.lTapTime = 0f;
            }
            if (this.lTapTime != 0f)
            {
                this.dashL = true;
            }
        }
        if (this.inputManager.isInputDown[InputCode.right])
        {
            if (this.rTapTime == -1f)
            {
                this.rTapTime = 0f;
            }
            if (this.rTapTime != 0f)
            {
                this.dashR = true;
            }
        }
    }

    public void continueAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
	            disposable.Dispose();
            }
        }
        this.customAnimationSpeed();
        this.playAnimation(this.currentPlayingClipName());
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && base.photonView.isMine)
        {
            base.photonView.RPC("netContinueAnimation", PhotonTargets.Others, new object[0]);
        }
    }

    public void crossFade(string aniName, float time)
    {
        this.currentAnimation = aniName;
        base.animation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, time };
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
        }
    }

    public string currentPlayingClipName()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (base.animation.IsPlaying(current.name))
                {
                    return current.name;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
	            disposable.Dispose();
            }
        }
        return string.Empty;
    }

    private void customAnimationSpeed()
    {
        base.animation["attack5"].speed = 1.85f;
        base.animation["changeBlade"].speed = 1.2f;
        base.animation["air_release"].speed = 0.6f;
        base.animation["changeBlade_air"].speed = 0.8f;
        base.animation["AHSS_gun_reload_both"].speed = 0.38f;
        base.animation["AHSS_gun_reload_both_air"].speed = 0.5f;
        base.animation["AHSS_gun_reload_l"].speed = 0.4f;
        base.animation["AHSS_gun_reload_l_air"].speed = 0.5f;
        base.animation["AHSS_gun_reload_r"].speed = 0.4f;
        base.animation["AHSS_gun_reload_r_air"].speed = 0.5f;
    }

    private void dash(float horizontal, float vertical)
    {
        UnityEngine.MonoBehaviour.print(this.dashTime + " " + this.currentGas);
        if (((this.dashTime <= 0f) && (this.currentGas > 0f)) && !this.isMounted)
        {
            this.useGas(this.totalGas * 0.04f);
            this.facingDirection = this.getGlobalFacingDirection(horizontal, vertical);
            this.dashV = this.getGlobaleFacingVector3(this.facingDirection);
            this.originVM = this.currentSpeed;
            Quaternion quaternion = Quaternion.Euler(0f, this.facingDirection, 0f);
            base.rigidbody.rotation = quaternion;
            this.targetRotation = quaternion;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                UnityEngine.Object.Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);
            }
            this.dashTime = 0.5f;
            this.crossFade("dash", 0.1f);
            base.animation["dash"].time = 0.1f;
            this.state = HERO_STATE.AirDodge;
            this.falseAttack();
            base.rigidbody.AddForce((Vector3) (this.dashV * 40f), ForceMode.VelocityChange);
        }
    }

    public void die(Vector3 v, bool isBite)
    {
        if (this.invincible <= 0f)
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().removeMe();
            }
            this.meatDie.Play();
            if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine) && !this.useGun)
            {
                this.leftbladetrail.Deactivate();
                this.rightbladetrail.Deactivate();
                this.leftbladetrail2.Deactivate();
                this.rightbladetrail2.Deactivate();
            }
            this.breakApart(v, isBite);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose();
            this.falseAttack();
            this.hasDied = true;
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot(base.transform.position, 0, null, 0.02f);
            }
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public void die2(Transform tf)
    {
        if (this.invincible <= 0f)
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().removeMe();
            }
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            this.meatDie.Play();
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose();
            this.falseAttack();
            this.hasDied = true;
            GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
            obj2.transform.position = base.transform.position;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void dodge(bool offTheWall = false)
    {
        if (((this.myHorse != null) && !this.isMounted) && (Vector3.Distance(this.myHorse.transform.position, base.transform.position) < 15f))
        {
            this.getOnHorse();
        }
        else
        {
            this.state = HERO_STATE.GroundDodge;
            if (!offTheWall)
            {
                float num;
                float num2;
                if (this.inputManager.isInput[InputCode.up])
                {
                    num2 = 1f;
                }
                else if (this.inputManager.isInput[InputCode.down])
                {
                    num2 = -1f;
                }
                else
                {
                    num2 = 0f;
                }
                if (this.inputManager.isInput[InputCode.left])
                {
                    num = -1f;
                }
                else if (this.inputManager.isInput[InputCode.right])
                {
                    num = 1f;
                }
                else
                {
                    num = 0f;
                }
                float num3 = this.getGlobalFacingDirection(num, num2);
                if ((num != 0f) || (num2 != 0f))
                {
                    this.facingDirection = num3 + 180f;
                    this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                }
                this.crossFade("dodge", 0.1f);
            }
            else
            {
                this.playAnimation("dodge");
                this.playAnimationAt("dodge", 0.2f);
            }
            this.sparks.enableEmission = false;
        }
    }

    private void erenTransform()
    {
        this.skillCDDuration = this.skillCDLast;
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            this.eren_titan = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), base.transform.position, base.transform.rotation);
        }
        else
        {
            this.eren_titan = PhotonNetwork.Instantiate("TITAN_EREN", base.transform.position, base.transform.rotation, 0);
        }
        this.eren_titan.GetComponent<TITAN_EREN>().realBody = base.gameObject;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.eren_titan, true, false);
        this.eren_titan.GetComponent<TITAN_EREN>().born();
        this.eren_titan.rigidbody.velocity = base.rigidbody.velocity;
        base.rigidbody.velocity = Vector3.zero;
        base.transform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
        this.titanForm = true;
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            object[] parameters = new object[] { this.eren_titan.GetPhotonView().viewID };
            base.photonView.RPC("whoIsMyErenTitan", PhotonTargets.Others, parameters);
        }
        if ((this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
        {
            object[] objArray2 = new object[] { false };
            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, objArray2);
        }
        this.smoke_3dmg.enableEmission = false;
    }

    private void escapeFromGrab()
    {
    }

    public void falseAttack()
    {
        this.attackMove = false;
        if (this.useGun)
        {
            if (!this.attackReleased)
            {
                this.continueAnimation();
                this.attackReleased = true;
            }
        }
        else
        {
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                this.leftbladetrail.StopSmoothly(0.2f);
                this.rightbladetrail.StopSmoothly(0.2f);
                this.leftbladetrail2.StopSmoothly(0.2f);
                this.rightbladetrail2.StopSmoothly(0.2f);
            }
            this.attackLoop = 0;
            if (!this.attackReleased)
            {
                this.continueAnimation();
                this.attackReleased = true;
            }
        }
    }

    public void fillGas()
    {
        this.currentGas = this.totalGas;
    }

    private GameObject findNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        Vector3 position = base.transform.position;
        foreach (GameObject obj3 in objArray)
        {
            Vector3 vector2 = obj3.transform.position - position;
            float sqrMagnitude = vector2.sqrMagnitude;
            if (sqrMagnitude < positiveInfinity)
            {
                obj2 = obj3;
                positiveInfinity = sqrMagnitude;
            }
        }
        return obj2;
    }

    private void FixedUpdate()
    {
        if (!this.titanForm && (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)))
        {
            this.currentSpeed = base.rigidbody.velocity.magnitude;
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                if (this.state == HERO_STATE.Grab)
                {
                    base.rigidbody.AddForce(-base.rigidbody.velocity, ForceMode.VelocityChange);
                }
                else
                {
                    if (this.IsGrounded())
                    {
                        if (!this.grounded)
                        {
                            this.justGrounded = true;
                        }
                        this.grounded = true;
                    }
                    else
                    {
                        this.grounded = false;
                    }
                    if (this.hookSomeOne)
                    {
                        if (this.hookTarget != null)
                        {
                            Vector3 vector = this.hookTarget.transform.position - base.transform.position;
                            float magnitude = vector.magnitude;
                            if (magnitude > 2f)
                            {
                                base.rigidbody.AddForce((Vector3) (((vector.normalized * Mathf.Pow(magnitude, 0.15f)) * 30f) - (base.rigidbody.velocity * 0.95f)), ForceMode.VelocityChange);
                            }
                        }
                        else
                        {
                            this.hookSomeOne = false;
                        }
                    }
                    else if (this.hookBySomeOne && (this.badGuy != null))
                    {
                        if (this.badGuy != null)
                        {
                            Vector3 vector2 = this.badGuy.transform.position - base.transform.position;
                            float f = vector2.magnitude;
                            if (f > 5f)
                            {
                                base.rigidbody.AddForce((Vector3) ((vector2.normalized * Mathf.Pow(f, 0.15f)) * 0.2f), ForceMode.Impulse);
                            }
                        }
                        else
                        {
                            this.hookBySomeOne = false;
                        }
                    }
                    float x = 0f;
                    float z = 0f;
                    if (!IN_GAME_MAIN_CAMERA.isTyping)
                    {
                        if (this.inputManager.isInput[InputCode.up])
                        {
                            z = 1f;
                        }
                        else if (this.inputManager.isInput[InputCode.down])
                        {
                            z = -1f;
                        }
                        else
                        {
                            z = 0f;
                        }
                        if (this.inputManager.isInput[InputCode.left])
                        {
                            x = -1f;
                        }
                        else if (this.inputManager.isInput[InputCode.right])
                        {
                            x = 1f;
                        }
                        else
                        {
                            x = 0f;
                        }
                    }
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    this.isLeftHandHooked = false;
                    this.isRightHandHooked = false;
                    if (this.isLaunchLeft)
                    {
                        if ((this.bulletLeft != null) && this.bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            this.isLeftHandHooked = true;
                            Vector3 to = this.bulletLeft.transform.position - base.transform.position;
                            to.Normalize();
                            to = (Vector3) (to * 10f);
                            if (!this.isLaunchRight)
                            {
                                to = (Vector3) (to * 2f);
                            }
                            if ((Vector3.Angle(base.rigidbody.velocity, to) > 90f) && this.inputManager.isInput[InputCode.jump])
                            {
                                flag2 = true;
                                flag = true;
                            }
                            if (!flag2)
                            {
                                base.rigidbody.AddForce(to);
                                if (Vector3.Angle(base.rigidbody.velocity, to) > 90f)
                                {
                                    base.rigidbody.AddForce((Vector3) (-base.rigidbody.velocity * 2f), ForceMode.Acceleration);
                                }
                            }
                        }
                        this.launchElapsedTimeL += Time.deltaTime;
                        if (this.QHold && (this.currentGas > 0f))
                        {
                            this.useGas(this.useGasSpeed * Time.deltaTime);
                        }
                        else if (this.launchElapsedTimeL > 0.3f)
                        {
                            this.isLaunchLeft = false;
                            if (this.bulletLeft != null)
                            {
                                this.bulletLeft.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                                this.bulletLeft = null;
                                flag2 = false;
                            }
                        }
                    }
                    if (this.isLaunchRight)
                    {
                        if ((this.bulletRight != null) && this.bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            this.isRightHandHooked = true;
                            Vector3 vector4 = this.bulletRight.transform.position - base.transform.position;
                            vector4.Normalize();
                            vector4 = (Vector3) (vector4 * 10f);
                            if (!this.isLaunchLeft)
                            {
                                vector4 = (Vector3) (vector4 * 2f);
                            }
                            if ((Vector3.Angle(base.rigidbody.velocity, vector4) > 90f) && this.inputManager.isInput[InputCode.jump])
                            {
                                flag3 = true;
                                flag = true;
                            }
                            if (!flag3)
                            {
                                base.rigidbody.AddForce(vector4);
                                if (Vector3.Angle(base.rigidbody.velocity, vector4) > 90f)
                                {
                                    base.rigidbody.AddForce((Vector3) (-base.rigidbody.velocity * 2f), ForceMode.Acceleration);
                                }
                            }
                        }
                        this.launchElapsedTimeR += Time.deltaTime;
                        if (this.EHold && (this.currentGas > 0f))
                        {
                            this.useGas(this.useGasSpeed * Time.deltaTime);
                        }
                        else if (this.launchElapsedTimeR > 0.3f)
                        {
                            this.isLaunchRight = false;
                            if (this.bulletRight != null)
                            {
                                this.bulletRight.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                                this.bulletRight = null;
                                flag3 = false;
                            }
                        }
                    }
                    if (this.grounded)
                    {
                        Vector3 vector26;
                        Vector3 zero = Vector3.zero;
                        if (this.state == HERO_STATE.Attack)
                        {
                            if (this.attackAnimation == "attack5")
                            {
                                if ((base.animation[this.attackAnimation].normalizedTime > 0.4f) && (base.animation[this.attackAnimation].normalizedTime < 0.61f))
                                {
                                    base.rigidbody.AddForce((Vector3) (base.gameObject.transform.forward * 200f));
                                }
                            }
                            else if (this.attackAnimation == "special_petra")
                            {
                                if ((base.animation[this.attackAnimation].normalizedTime > 0.35f) && (base.animation[this.attackAnimation].normalizedTime < 0.48f))
                                {
                                    base.rigidbody.AddForce((Vector3) (base.gameObject.transform.forward * 200f));
                                }
                            }
                            else if (base.animation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                            else if (base.animation.IsPlaying("attack1") || base.animation.IsPlaying("attack2"))
                            {
                                base.rigidbody.AddForce((Vector3) (base.gameObject.transform.forward * 200f));
                            }
                            if (base.animation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (this.justGrounded)
                        {
                            if ((this.state != HERO_STATE.Attack) || (((this.attackAnimation != "attack3_1") && (this.attackAnimation != "attack5")) && (this.attackAnimation != "special_petra")))
                            {
                                if ((((this.state != HERO_STATE.Attack) && (x == 0f)) && ((z == 0f) && (this.bulletLeft == null))) && ((this.bulletRight == null) && (this.state != HERO_STATE.FillGas)))
                                {
                                    this.state = HERO_STATE.Land;
                                    this.crossFade("dash_land", 0.01f);
                                }
                                else
                                {
                                    this.buttonAttackRelease = true;
                                    if (((this.state != HERO_STATE.Attack) && (((base.rigidbody.velocity.x * base.rigidbody.velocity.x) + (base.rigidbody.velocity.z * base.rigidbody.velocity.z)) > ((this.speed * this.speed) * 1.5f))) && (this.state != HERO_STATE.FillGas))
                                    {
                                        this.state = HERO_STATE.Slide;
                                        this.crossFade("slide", 0.05f);
                                        this.facingDirection = Mathf.Atan2(base.rigidbody.velocity.x, base.rigidbody.velocity.z) * 57.29578f;
                                        this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                        this.sparks.enableEmission = true;
                                    }
                                }
                            }
                            this.justGrounded = false;
                            zero = base.rigidbody.velocity;
                        }
                        if (((this.state == HERO_STATE.Attack) && (this.attackAnimation == "attack3_1")) && (base.animation[this.attackAnimation].normalizedTime >= 1f))
                        {
                            this.playAnimation("attack3_2");
                            this.resetAnimationSpeed();
                            vector26 = Vector3.zero;
                            base.rigidbody.velocity = vector26;
                            zero = vector26;
                            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
                        }
                        if (this.state == HERO_STATE.GroundDodge)
                        {
                            if ((base.animation["dodge"].normalizedTime >= 0.2f) && (base.animation["dodge"].normalizedTime < 0.8f))
                            {
                                zero = (Vector3) ((-base.transform.forward * 2.4f) * this.speed);
                            }
                            if (base.animation["dodge"].normalizedTime > 0.8f)
                            {
                                zero = (Vector3) (base.rigidbody.velocity * 0.9f);
                            }
                        }
                        else if (this.state == HERO_STATE.Idle)
                        {
                            Vector3 vector6 = new Vector3(x, 0f, z);
                            float resultAngle = this.getGlobalFacingDirection(x, z);
                            zero = this.getGlobaleFacingVector3(resultAngle);
                            float num6 = (vector6.magnitude <= 0.95f) ? ((vector6.magnitude >= 0.25f) ? vector6.magnitude : 0f) : 1f;
                            zero = (Vector3) (zero * num6);
                            zero = (Vector3) (zero * this.speed);
                            if ((this.buffTime > 0f) && (this.currentBuff == BUFF.SpeedUp))
                            {
                                zero = (Vector3) (zero * 4f);
                            }
                            if ((x != 0f) || (z != 0f))
                            {
                                if (((!base.animation.IsPlaying("run") && !base.animation.IsPlaying("jump")) && !base.animation.IsPlaying("run_sasha")) && (!base.animation.IsPlaying("horse_geton") || (base.animation["horse_geton"].normalizedTime >= 0.5f)))
                                {
                                    if ((this.buffTime > 0f) && (this.currentBuff == BUFF.SpeedUp))
                                    {
                                        this.crossFade("run_sasha", 0.1f);
                                    }
                                    else
                                    {
                                        this.crossFade("run", 0.1f);
                                    }
                                }
                            }
                            else
                            {
                                if (((!base.animation.IsPlaying(this.standAnimation) && (this.state != HERO_STATE.Land)) && (!base.animation.IsPlaying("jump") && !base.animation.IsPlaying("horse_geton"))) && !base.animation.IsPlaying("grabbed"))
                                {
                                    this.crossFade(this.standAnimation, 0.1f);
                                    zero = (Vector3) (zero * 0f);
                                }
                                resultAngle = -874f;
                            }
                            if (resultAngle != -874f)
                            {
                                this.facingDirection = resultAngle;
                                this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                            }
                        }
                        else if (this.state == HERO_STATE.Land)
                        {
                            zero = (Vector3) (base.rigidbody.velocity * 0.96f);
                        }
                        else if (this.state == HERO_STATE.Slide)
                        {
                            zero = (Vector3) (base.rigidbody.velocity * 0.99f);
                            if (this.currentSpeed < (this.speed * 1.2f))
                            {
                                this.idle();
                                this.sparks.enableEmission = false;
                            }
                        }
                        Vector3 velocity = base.rigidbody.velocity;
                        Vector3 force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -this.maxVelocityChange, this.maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -this.maxVelocityChange, this.maxVelocityChange);
                        force.y = 0f;
                        if (base.animation.IsPlaying("jump") && (base.animation["jump"].normalizedTime > 0.18f))
                        {
                            force.y += 8f;
                        }
                        if ((base.animation.IsPlaying("horse_geton") && (base.animation["horse_geton"].normalizedTime > 0.18f)) && (base.animation["horse_geton"].normalizedTime < 1f))
                        {
                            float num7 = 6f;
                            force = -base.rigidbody.velocity;
                            force.y = num7;
                            float num8 = Vector3.Distance(this.myHorse.transform.position, base.transform.position);
                            float num9 = ((0.6f * this.gravity) * num8) / (2f * num7);
                            vector26 = this.myHorse.transform.position - base.transform.position;
                            force += (Vector3) (num9 * vector26.normalized);
                        }
                        if ((this.state != HERO_STATE.Attack) || !this.useGun)
                        {
                            base.rigidbody.AddForce(force, ForceMode.VelocityChange);
                            base.rigidbody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (this.sparks.enableEmission)
                        {
                            this.sparks.enableEmission = false;
                        }
                        if (((this.myHorse != null) && (base.animation.IsPlaying("horse_geton") || base.animation.IsPlaying("air_fall"))) && ((base.rigidbody.velocity.y < 0f) && (Vector3.Distance(this.myHorse.transform.position + ((Vector3) (Vector3.up * 1.65f)), base.transform.position) < 0.5f)))
                        {
                            base.transform.position = this.myHorse.transform.position + ((Vector3) (Vector3.up * 1.65f));
                            base.transform.rotation = this.myHorse.transform.rotation;
                            this.isMounted = true;
                            this.crossFade("horse_idle", 0.1f);
                            this.myHorse.GetComponent<Horse>().mounted();
                        }
                        if ((((((this.state == HERO_STATE.Idle) && !base.animation.IsPlaying("dash")) && (!base.animation.IsPlaying("wallrun") && !base.animation.IsPlaying("toRoof"))) && ((!base.animation.IsPlaying("horse_geton") && !base.animation.IsPlaying("horse_getoff")) && (!base.animation.IsPlaying("air_release") && !this.isMounted))) && ((!base.animation.IsPlaying("air_hook_l_just") || (base.animation["air_hook_l_just"].normalizedTime >= 1f)) && (!base.animation.IsPlaying("air_hook_r_just") || (base.animation["air_hook_r_just"].normalizedTime >= 1f)))) || (base.animation["dash"].normalizedTime >= 0.99f))
                        {
                            if (((!this.isLeftHandHooked && !this.isRightHandHooked) && ((base.animation.IsPlaying("air_hook_l") || base.animation.IsPlaying("air_hook_r")) || base.animation.IsPlaying("air_hook"))) && (base.rigidbody.velocity.y > 20f))
                            {
                                base.animation.CrossFade("air_release");
                            }
                            else
                            {
                                bool flag4 = (Mathf.Abs(base.rigidbody.velocity.x) + Mathf.Abs(base.rigidbody.velocity.z)) > 25f;
                                bool flag5 = base.rigidbody.velocity.y < 0f;
                                if (!flag4)
                                {
                                    if (flag5)
                                    {
                                        if (!base.animation.IsPlaying("air_fall"))
                                        {
                                            this.crossFade("air_fall", 0.2f);
                                        }
                                    }
                                    else if (!base.animation.IsPlaying("air_rise"))
                                    {
                                        this.crossFade("air_rise", 0.2f);
                                    }
                                }
                                else if (!this.isLeftHandHooked && !this.isRightHandHooked)
                                {
                                    float current = -Mathf.Atan2(base.rigidbody.velocity.z, base.rigidbody.velocity.x) * 57.29578f;
                                    float num11 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!base.animation.IsPlaying("air2"))
                                        {
                                            this.crossFade("air2", 0.2f);
                                        }
                                    }
                                    else if ((num11 < 135f) && (num11 > 0f))
                                    {
                                        if (!base.animation.IsPlaying("air2_right"))
                                        {
                                            this.crossFade("air2_right", 0.2f);
                                        }
                                    }
                                    else if ((num11 > -135f) && (num11 < 0f))
                                    {
                                        if (!base.animation.IsPlaying("air2_left"))
                                        {
                                            this.crossFade("air2_left", 0.2f);
                                        }
                                    }
                                    else if (!base.animation.IsPlaying("air2_backward"))
                                    {
                                        this.crossFade("air2_backward", 0.2f);
                                    }
                                }
                                else if (this.useGun)
                                {
                                    if (!this.isRightHandHooked)
                                    {
                                        if (!base.animation.IsPlaying("AHSS_hook_forward_l"))
                                        {
                                            this.crossFade("AHSS_hook_forward_l", 0.1f);
                                        }
                                    }
                                    else if (!this.isLeftHandHooked)
                                    {
                                        if (!base.animation.IsPlaying("AHSS_hook_forward_r"))
                                        {
                                            this.crossFade("AHSS_hook_forward_r", 0.1f);
                                        }
                                    }
                                    else if (!base.animation.IsPlaying("AHSS_hook_forward_both"))
                                    {
                                        this.crossFade("AHSS_hook_forward_both", 0.1f);
                                    }
                                }
                                else if (!this.isRightHandHooked)
                                {
                                    if (!base.animation.IsPlaying("air_hook_l"))
                                    {
                                        this.crossFade("air_hook_l", 0.1f);
                                    }
                                }
                                else if (!this.isLeftHandHooked)
                                {
                                    if (!base.animation.IsPlaying("air_hook_r"))
                                    {
                                        this.crossFade("air_hook_r", 0.1f);
                                    }
                                }
                                else if (!base.animation.IsPlaying("air_hook"))
                                {
                                    this.crossFade("air_hook", 0.1f);
                                }
                            }
                        }
                        if (((this.state == HERO_STATE.Idle) && base.animation.IsPlaying("air_release")) && (base.animation["air_release"].normalizedTime >= 1f))
                        {
                            this.crossFade("air_rise", 0.2f);
                        }
                        if (base.animation.IsPlaying("horse_getoff") && (base.animation["horse_getoff"].normalizedTime >= 1f))
                        {
                            this.crossFade("air_rise", 0.2f);
                        }
                        if (base.animation.IsPlaying("toRoof"))
                        {
                            if (base.animation["toRoof"].normalizedTime < 0.22f)
                            {
                                base.rigidbody.velocity = Vector3.zero;
                                base.rigidbody.AddForce(new Vector3(0f, this.gravity * base.rigidbody.mass, 0f));
                            }
                            else
                            {
                                if (!this.wallJump)
                                {
                                    this.wallJump = true;
                                    base.rigidbody.AddForce((Vector3) (Vector3.up * 8f), ForceMode.Impulse);
                                }
                                base.rigidbody.AddForce((Vector3) (base.transform.forward * 0.05f), ForceMode.Impulse);
                            }
                            if (base.animation["toRoof"].normalizedTime >= 1f)
                            {
                                this.playAnimation("air_rise");
                            }
                        }
                        else if (((((this.state == HERO_STATE.Idle) && this.isPressDirectionTowardsHero(x, z)) && (!this.inputManager.isInput[InputCode.jump] && !this.inputManager.isInput[InputCode.leftRope])) && ((!this.inputManager.isInput[InputCode.rightRope] && !this.inputManager.isInput[InputCode.bothRope]) && (this.IsFrontGrounded() && !base.animation.IsPlaying("wallrun")))) && !base.animation.IsPlaying("dodge"))
                        {
                            this.crossFade("wallrun", 0.1f);
                            this.wallRunTime = 0f;
                        }
                        else if (base.animation.IsPlaying("wallrun"))
                        {
                            base.rigidbody.AddForce(((Vector3) (Vector3.up * this.speed)) - base.rigidbody.velocity, ForceMode.VelocityChange);
                            this.wallRunTime += Time.deltaTime;
                            if ((this.wallRunTime > 1f) || ((z == 0f) && (x == 0f)))
                            {
                                base.rigidbody.AddForce((Vector3) ((-base.transform.forward * this.speed) * 0.75f), ForceMode.Impulse);
                                this.dodge(true);
                            }
                            else if (!this.IsUpFrontGrounded())
                            {
                                this.wallJump = false;
                                this.crossFade("toRoof", 0.1f);
                            }
                            else if (!this.IsFrontGrounded())
                            {
                                this.crossFade("air_fall", 0.1f);
                            }
                        }
                        else if ((!base.animation.IsPlaying("attack5") && !base.animation.IsPlaying("special_petra")) && (!base.animation.IsPlaying("dash") && !base.animation.IsPlaying("jump")))
                        {
                            Vector3 vector9 = new Vector3(x, 0f, z);
                            float num12 = this.getGlobalFacingDirection(x, z);
                            Vector3 vector10 = this.getGlobaleFacingVector3(num12);
                            float num13 = (vector9.magnitude <= 0.95f) ? ((vector9.magnitude >= 0.25f) ? vector9.magnitude : 0f) : 1f;
                            vector10 = (Vector3) (vector10 * num13);
                            vector10 = (Vector3) (vector10 * ((((float) this.setup.myCostume.stat.ACL) / 10f) * 2f));
                            if ((x == 0f) && (z == 0f))
                            {
                                if (this.state == HERO_STATE.Attack)
                                {
                                    vector10 = (Vector3) (vector10 * 0f);
                                }
                                num12 = -874f;
                            }
                            if (num12 != -874f)
                            {
                                this.facingDirection = num12;
                                this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                            }
                            if (((!flag2 && !flag3) && (!this.isMounted && this.inputManager.isInput[InputCode.jump])) && (this.currentGas > 0f))
                            {
                                if ((x != 0f) || (z != 0f))
                                {
                                    base.rigidbody.AddForce(vector10, ForceMode.Acceleration);
                                }
                                else
                                {
                                    base.rigidbody.AddForce((Vector3) (base.transform.forward * vector10.magnitude), ForceMode.Acceleration);
                                }
                                flag = true;
                            }
                        }
                        if ((base.animation.IsPlaying("air_fall") && (this.currentSpeed < 0.2f)) && this.IsFrontGrounded())
                        {
                            this.crossFade("onWall", 0.3f);
                        }
                    }
                    this.spinning = false;
                    if (flag2 && flag3)
                    {
                        float num14 = this.currentSpeed + 0.1f;
                        base.rigidbody.AddForce(-base.rigidbody.velocity, ForceMode.VelocityChange);
                        Vector3 vector11 = ((Vector3) ((this.bulletRight.transform.position + this.bulletLeft.transform.position) * 0.5f)) - base.transform.position;
                        float num15 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        num15 = Mathf.Clamp(num15, -0.8f, 0.8f);
                        float num16 = 1f + num15;
                        Vector3 vector12 = Vector3.RotateTowards(vector11, base.rigidbody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector12.Normalize();
                        this.spinning = true;
                        base.rigidbody.velocity = (Vector3) (vector12 * num14);
                    }
                    else if (flag2)
                    {
                        float num17 = this.currentSpeed + 0.1f;
                        base.rigidbody.AddForce(-base.rigidbody.velocity, ForceMode.VelocityChange);
                        Vector3 vector13 = this.bulletLeft.transform.position - base.transform.position;
                        float num18 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        num18 = Mathf.Clamp(num18, -0.8f, 0.8f);
                        float num19 = 1f + num18;
                        Vector3 vector14 = Vector3.RotateTowards(vector13, base.rigidbody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector14.Normalize();
                        this.spinning = true;
                        base.rigidbody.velocity = (Vector3) (vector14 * num17);
                    }
                    else if (flag3)
                    {
                        float num20 = this.currentSpeed + 0.1f;
                        base.rigidbody.AddForce(-base.rigidbody.velocity, ForceMode.VelocityChange);
                        Vector3 vector15 = this.bulletRight.transform.position - base.transform.position;
                        float num21 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        num21 = Mathf.Clamp(num21, -0.8f, 0.8f);
                        float num22 = 1f + num21;
                        Vector3 vector16 = Vector3.RotateTowards(vector15, base.rigidbody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector16.Normalize();
                        this.spinning = true;
                        base.rigidbody.velocity = (Vector3) (vector16 * num20);
                    }
                    if (((this.state == HERO_STATE.Attack) && ((this.attackAnimation == "attack5") || (this.attackAnimation == "special_petra"))) && ((base.animation[this.attackAnimation].normalizedTime > 0.4f) && !this.attackMove))
                    {
                        this.attackMove = true;
                        if (this.launchPointRight.magnitude > 0f)
                        {
                            Vector3 vector17 = this.launchPointRight - base.transform.position;
                            vector17.Normalize();
                            vector17 = (Vector3) (vector17 * 13f);
                            base.rigidbody.AddForce(vector17, ForceMode.Impulse);
                        }
                        if ((this.attackAnimation == "special_petra") && (this.launchPointLeft.magnitude > 0f))
                        {
                            Vector3 vector18 = this.launchPointLeft - base.transform.position;
                            vector18.Normalize();
                            vector18 = (Vector3) (vector18 * 13f);
                            base.rigidbody.AddForce(vector18, ForceMode.Impulse);
                            if (this.bulletRight != null)
                            {
                                this.bulletRight.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                            }
                            if (this.bulletLeft != null)
                            {
                                this.bulletLeft.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                            }
                        }
                        base.rigidbody.AddForce((Vector3) (Vector3.up * 2f), ForceMode.Impulse);
                    }
                    bool flag6 = false;
                    if ((this.bulletLeft != null) || (this.bulletRight != null))
                    {
                        if (((this.bulletLeft != null) && (this.bulletLeft.transform.position.y > base.gameObject.transform.position.y)) && (this.isLaunchLeft && this.bulletLeft.GetComponent<Bullet>().isHooked()))
                        {
                            flag6 = true;
                        }
                        if (((this.bulletRight != null) && (this.bulletRight.transform.position.y > base.gameObject.transform.position.y)) && (this.isLaunchRight && this.bulletRight.GetComponent<Bullet>().isHooked()))
                        {
                            flag6 = true;
                        }
                    }
                    if (flag6)
                    {
                        base.rigidbody.AddForce(new Vector3(0f, -10f * base.rigidbody.mass, 0f));
                    }
                    else
                    {
                        base.rigidbody.AddForce(new Vector3(0f, -this.gravity * base.rigidbody.mass, 0f));
                    }
                    if (this.currentSpeed > 10f)
                    {
                        this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min((float) 100f, (float) (this.currentSpeed + 40f)), 0.1f);
                    }
                    else
                    {
                        this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                    }
                    if (flag)
                    {
                        this.useGas(this.useGasSpeed * Time.deltaTime);
                        if ((!this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
                        {
                            object[] parameters = new object[] { true };
                            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
                        }
                        this.smoke_3dmg.enableEmission = true;
                    }
                    else
                    {
                        if ((this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
                        {
                            object[] objArray2 = new object[] { false };
                            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, objArray2);
                        }
                        this.smoke_3dmg.enableEmission = false;
                    }
                    if (this.currentSpeed > 80f)
                    {
                        if (!this.speedFXPS.enableEmission)
                        {
                            this.speedFXPS.enableEmission = true;
                        }
                        this.speedFXPS.startSpeed = this.currentSpeed;
                        this.speedFX.transform.LookAt(base.transform.position + base.rigidbody.velocity);
                    }
                    else if (this.speedFXPS.enableEmission)
                    {
                        this.speedFXPS.enableEmission = false;
                    }
                }
            }
        }
    }

    public string getDebugInfo()
    {
        string str = "\n";
        str = "Left:" + this.isLeftHandHooked + " ";
        if (this.isLeftHandHooked && (this.bulletLeft != null))
        {
            Vector3 vector = this.bulletLeft.transform.position - base.transform.position;
            str = str + ((int) (Mathf.Atan2(vector.x, vector.z) * 57.29578f));
        }
        string str2 = str;
        object[] objArray1 = new object[] { str2, "\nRight:", this.isRightHandHooked, " " };
        str = string.Concat(objArray1);
        if (this.isRightHandHooked && (this.bulletRight != null))
        {
            Vector3 vector2 = this.bulletRight.transform.position - base.transform.position;
            str = str + ((int) (Mathf.Atan2(vector2.x, vector2.z) * 57.29578f));
        }
        str = (((str + "\nfacingDirection:" + ((int) this.facingDirection)) + "\nActual facingDirection:" + ((int) base.transform.rotation.eulerAngles.y)) + "\nState:" + this.state.ToString()) + "\n\n\n\n\n";
        if (this.state == HERO_STATE.Attack)
        {
            this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
        }
        return str;
    }

    private Vector3 getGlobaleFacingVector3(float resultAngle)
    {
        float num = -resultAngle + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private Vector3 getGlobaleFacingVector3(float horizontal, float vertical)
    {
        float num = -this.getGlobalFacingDirection(horizontal, vertical) + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private float getGlobalFacingDirection(float horizontal, float vertical)
    {
        if ((vertical == 0f) && (horizontal == 0f))
        {
            return base.transform.rotation.eulerAngles.y;
        }
        float y = this.currentCamera.transform.rotation.eulerAngles.y;
        float num2 = Mathf.Atan2(vertical, horizontal) * 57.29578f;
        num2 = -num2 + 90f;
        return (y + num2);
    }

    private float getLeanAngle(Vector3 p, bool left)
    {
        if (!this.useGun && (this.state == HERO_STATE.Attack))
        {
            return 0f;
        }
        float num = p.y - base.transform.position.y;
        float num2 = Vector3.Distance(p, base.transform.position);
        float a = Mathf.Acos(num / num2) * 57.29578f;
        a *= 0.1f;
        a *= 1f + Mathf.Pow(base.rigidbody.velocity.magnitude, 0.2f);
        Vector3 vector = p - base.transform.position;
        float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
        float target = Mathf.Atan2(base.rigidbody.velocity.x, base.rigidbody.velocity.z) * 57.29578f;
        float num6 = Mathf.DeltaAngle(current, target);
        a += Mathf.Abs((float) (num6 * 0.5f));
        if (this.state != HERO_STATE.Attack)
        {
            a = Mathf.Min(a, 80f);
        }
        if (num6 > 0f)
        {
            this.leanLeft = true;
        }
        else
        {
            this.leanLeft = false;
        }
        if (this.useGun)
        {
            return (a * ((num6 >= 0f) ? ((float) 1) : ((float) (-1))));
        }
        float num7 = 0f;
        if ((left && (num6 < 0f)) || (!left && (num6 > 0f)))
        {
            num7 = 0.1f;
        }
        else
        {
            num7 = 0.5f;
        }
        return (a * ((num6 >= 0f) ? num7 : -num7));
    }

    private void getOffHorse()
    {
        this.playAnimation("horse_getoff");
        base.rigidbody.AddForce((Vector3) (((Vector3.up * 10f) - (base.transform.forward * 2f)) - (base.transform.right * 1f)), ForceMode.VelocityChange);
        this.unmounted();
    }

    private void getOnHorse()
    {
        this.playAnimation("horse_geton");
        this.facingDirection = this.myHorse.transform.rotation.eulerAngles.y;
        this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
    }

    public void getSupply()
    {
        if (((base.animation.IsPlaying(this.standAnimation) || base.animation.IsPlaying("run")) || base.animation.IsPlaying("run_sasha")) && (((this.currentBladeSta != this.totalBladeSta) || (this.currentBladeNum != this.totalBladeNum)) || (((this.currentGas != this.totalGas) || (this.leftBulletLeft != this.bulletMAX)) || (this.rightBulletLeft != this.bulletMAX))))
        {
            this.state = HERO_STATE.FillGas;
            this.crossFade("supply", 0.1f);
        }
    }

    public void grabbed(GameObject titan, bool leftHand)
    {
        if (this.isMounted)
        {
            this.unmounted();
        }
        this.state = HERO_STATE.Grab;
        base.GetComponent<CapsuleCollider>().isTrigger = true;
        this.falseAttack();
        this.titanWhoGrabMe = titan;
        if (this.titanForm && (this.eren_titan != null))
        {
            this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (!this.useGun && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            this.leftbladetrail.Deactivate();
            this.rightbladetrail.Deactivate();
            this.leftbladetrail2.Deactivate();
            this.rightbladetrail2.Deactivate();
        }
        this.smoke_3dmg.enableEmission = false;
        this.sparks.enableEmission = false;
    }

    public bool HasDied()
    {
        return (this.hasDied || this.isInvincible());
    }

    private void headMovement()
    {
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
        float x = Mathf.Sqrt(((this.gunTarget.x - base.transform.position.x) * (this.gunTarget.x - base.transform.position.x)) + ((this.gunTarget.z - base.transform.position.z) * (this.gunTarget.z - base.transform.position.z)));
        this.targetHeadRotation = transform.rotation;
        Vector3 vector = this.gunTarget - base.transform.position;
        float current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        float num3 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
        num3 = Mathf.Clamp(num3, -40f, 40f);
        float y = transform2.position.y - this.gunTarget.y;
        float num5 = Mathf.Atan2(y, x) * 57.29578f;
        num5 = Mathf.Clamp(num5, -40f, 30f);
        this.targetHeadRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + num5, transform.rotation.eulerAngles.y + num3, transform.rotation.eulerAngles.z);
        this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 60f);
        transform.rotation = this.oldHeadRotation;
    }

    public void hookedByHuman(int hooker, Vector3 hookPosition)
    {
        object[] parameters = new object[] { hooker, hookPosition };
        base.photonView.RPC("RPCHookedByHuman", base.photonView.owner, parameters);
    }

    [RPC]
    public void hookFail()
    {
        this.hookTarget = null;
        this.hookSomeOne = false;
    }

    public void hookToHuman(GameObject target, Vector3 hookPosition)
    {
        this.releaseIfIHookSb();
        this.hookTarget = target;
        this.hookSomeOne = true;
        if (target.GetComponent<HERO>() != null)
        {
            target.GetComponent<HERO>().hookedByHuman(base.photonView.viewID, hookPosition);
        }
        this.launchForce = hookPosition - base.transform.position;
        float num = Mathf.Pow(this.launchForce.magnitude, 0.1f);
        if (this.grounded)
        {
            base.rigidbody.AddForce((Vector3) (Vector3.up * Mathf.Min((float) (this.launchForce.magnitude * 0.2f), (float) 10f)), ForceMode.Impulse);
        }
        base.rigidbody.AddForce((Vector3) ((this.launchForce * num) * 0.1f), ForceMode.Impulse);
    }

    private void idle()
    {
        if (this.state == HERO_STATE.Attack)
        {
            this.falseAttack();
        }
        this.state = HERO_STATE.Idle;
        this.crossFade(this.standAnimation, 0.1f);
    }

    private bool IsFrontGrounded()
    {
        LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3) (base.gameObject.transform.up * 1f)), base.gameObject.transform.forward, (float) 1f, mask3.value);
    }

    public bool IsGrounded()
    {
        LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3) (Vector3.up * 0.1f)), -Vector3.up, (float) 0.3f, mask3.value);
    }

    public bool isInvincible()
    {
        return (this.invincible > 0f);
    }

    private bool isPressDirectionTowardsHero(float h, float v)
    {
        if ((h == 0f) && (v == 0f))
        {
            return false;
        }
        return (Mathf.Abs(Mathf.DeltaAngle(this.getGlobalFacingDirection(h, v), base.transform.rotation.eulerAngles.y)) < 45f);
    }

    private bool IsUpFrontGrounded()
    {
        LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3) (base.gameObject.transform.up * 3f)), base.gameObject.transform.forward, (float) 1.2f, mask3.value);
    }

    [RPC]
    private void killObject()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public void lateUpdate()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && (this.myNetWorkName != null))
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.myNetWorkName.transform.localPosition = (Vector3) ((Vector3.up * Screen.height) * 2f);
            }
            Vector3 start = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
            GameObject obj2 = GameObject.Find("MainCamera");
            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if ((Vector3.Angle(obj2.transform.forward, start - obj2.transform.position) > 90f) || Physics.Linecast(start, obj2.transform.position, (int) mask3))
            {
                this.myNetWorkName.transform.localPosition = (Vector3) ((Vector3.up * Screen.height) * 2f);
            }
            else
            {
                Vector2 vector2 = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(start);
                this.myNetWorkName.transform.localPosition = new Vector3((float) ((int) (vector2.x - (Screen.width * 0.5f))), (float) ((int) (vector2.y - (Screen.height * 0.5f))), 0f);
            }
        }
        if (!this.titanForm)
        {
            if ((IN_GAME_MAIN_CAMERA.cameraTilt == 1) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
            {
                Quaternion quaternion;
                Vector3 zero = Vector3.zero;
                Vector3 position = Vector3.zero;
                if ((this.isLaunchLeft && (this.bulletLeft != null)) && this.bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = this.bulletLeft.transform.position;
                }
                if ((this.isLaunchRight && (this.bulletRight != null)) && this.bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = this.bulletRight.transform.position;
                }
                Vector3 vector5 = Vector3.zero;
                if ((zero.magnitude != 0f) && (position.magnitude == 0f))
                {
                    vector5 = zero;
                }
                else if ((zero.magnitude == 0f) && (position.magnitude != 0f))
                {
                    vector5 = position;
                }
                else if ((zero.magnitude != 0f) && (position.magnitude != 0f))
                {
                    vector5 = (Vector3) ((zero + position) * 0.5f);
                }
                Vector3 from = Vector3.Project(vector5 - base.transform.position, GameObject.Find("MainCamera").transform.up);
                Vector3 vector7 = Vector3.Project(vector5 - base.transform.position, GameObject.Find("MainCamera").transform.right);
                if (vector5.magnitude > 0f)
                {
                    Vector3 to = from + vector7;
                    float num = Vector3.Angle(vector5 - base.transform.position, base.rigidbody.velocity) * 0.005f;
                    Vector3 vector15 = GameObject.Find("MainCamera").transform.right + vector7.normalized;
                    quaternion = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, (vector15.magnitude >= 1f) ? (-Vector3.Angle(from, to) * num) : (Vector3.Angle(from, to) * num));
                }
                else
                {
                    quaternion = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, 0f);
                }
                GameObject.Find("MainCamera").transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera").transform.rotation, quaternion, Time.deltaTime * 2f);
            }
            if ((this.state == HERO_STATE.Grab) && (this.titanWhoGrabMe != null))
            {
                if (this.titanWhoGrabMe.GetComponent<TITAN>() != null)
                {
                    base.transform.position = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
                    base.transform.rotation = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
                }
                else if (this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
                {
                    base.transform.position = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
                    base.transform.rotation = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
                }
            }
            if (this.useGun)
            {
                if (this.leftArmAim || this.rightArmAim)
                {
                    Vector3 vector9 = this.gunTarget - base.transform.position;
                    float current = -Mathf.Atan2(vector9.z, vector9.x) * 57.29578f;
                    float num3 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
                    this.headMovement();
                    if ((!this.isLeftHandHooked && this.leftArmAim) && ((num3 < 40f) && (num3 > -90f)))
                    {
                        this.leftArmAimTo(this.gunTarget);
                    }
                    if ((!this.isRightHandHooked && this.rightArmAim) && ((num3 > -40f) && (num3 < 90f)))
                    {
                        this.rightArmAimTo(this.gunTarget);
                    }
                }
                else if (!this.grounded)
                {
                    this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                if (this.isLeftHandHooked && (this.bulletLeft != null))
                {
                    this.leftArmAimTo(this.bulletLeft.transform.position);
                }
                if (this.isRightHandHooked && (this.bulletRight != null))
                {
                    this.rightArmAimTo(this.bulletRight.transform.position);
                }
            }
            this.setHookedPplDirection();
            this.bodyLean();
            if ((!base.animation.IsPlaying("attack3_2") && !base.animation.IsPlaying("attack5")) && !base.animation.IsPlaying("special_petra"))
            {
                base.rigidbody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, this.targetRotation, Time.deltaTime * 6f);
            }
        }
    }

    public void launch(Vector3 des, bool left = true, bool leviMode = false)
    {
        if (this.isMounted)
        {
            this.unmounted();
        }
        if (this.state != HERO_STATE.Attack)
        {
            this.idle();
        }
        Vector3 vector = des - base.transform.position;
        if (left)
        {
            this.launchPointLeft = des;
        }
        else
        {
            this.launchPointRight = des;
        }
        vector.Normalize();
        vector = (Vector3) (vector * 20f);
        if (((this.bulletLeft != null) && (this.bulletRight != null)) && (this.bulletLeft.GetComponent<Bullet>().isHooked() && this.bulletRight.GetComponent<Bullet>().isHooked()))
        {
            vector = (Vector3) (vector * 0.8f);
        }
        if (base.animation.IsPlaying("attack5") || base.animation.IsPlaying("special_petra"))
        {
            leviMode = true;
        }
        else
        {
            leviMode = false;
        }
        if (!leviMode)
        {
            this.falseAttack();
            this.idle();
            if (this.useGun)
            {
                this.crossFade("AHSS_hook_forward_both", 0.1f);
            }
            else if (left && !this.isRightHandHooked)
            {
                this.crossFade("air_hook_l_just", 0.1f);
            }
            else if (!left && !this.isLeftHandHooked)
            {
                this.crossFade("air_hook_r_just", 0.1f);
            }
            else
            {
                this.crossFade("dash", 0.1f);
                base.animation["dash"].time = 0f;
            }
        }
        if (left)
        {
            this.isLaunchLeft = true;
        }
        if (!left)
        {
            this.isLaunchRight = true;
        }
        this.launchForce = vector;
        if (!leviMode)
        {
            if (vector.y < 30f)
            {
                this.launchForce += (Vector3) (Vector3.up * (30f - vector.y));
            }
            if (des.y >= base.transform.position.y)
            {
                this.launchForce += (Vector3) ((Vector3.up * (des.y - base.transform.position.y)) * 10f);
            }
            base.rigidbody.AddForce(this.launchForce);
        }
        this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
        Quaternion quaternion = Quaternion.Euler(0f, this.facingDirection, 0f);
        base.gameObject.transform.rotation = quaternion;
        base.rigidbody.rotation = quaternion;
        this.targetRotation = quaternion;
        if (left)
        {
            this.launchElapsedTimeL = 0f;
        }
        else
        {
            this.launchElapsedTimeR = 0f;
        }
        if (leviMode)
        {
            this.launchElapsedTimeR = -100f;
        }
        if (base.animation.IsPlaying("special_petra"))
        {
            this.launchElapsedTimeR = -100f;
            this.launchElapsedTimeL = -100f;
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
            }
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
            }
        }
        this.sparks.enableEmission = false;
    }

    private void launchLeftRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (this.currentGas != 0f)
        {
            this.useGas(0f);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                this.bulletLeft = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                this.bulletLeft = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject obj2 = !this.useGun ? this.hookRefL1 : this.hookRefL2;
            string str = !this.useGun ? "hookRefL1" : "hookRefL2";
            this.bulletLeft.transform.position = obj2.transform.position;
            Bullet component = this.bulletLeft.GetComponent<Bullet>();
            float num = !single ? ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)) : 0f;
            Vector3 vector = (hit.point - ((Vector3) (base.transform.right * num))) - this.bulletLeft.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch((Vector3) (vector * 3f), base.rigidbody.velocity, str, true, base.gameObject, true);
            }
            else
            {
                component.launch((Vector3) (vector * 3f), base.rigidbody.velocity, str, true, base.gameObject, false);
            }
            this.launchPointLeft = Vector3.zero;
        }
    }

    private void launchRightRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (this.currentGas != 0f)
        {
            this.useGas(0f);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                this.bulletRight = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                this.bulletRight = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject obj2 = !this.useGun ? this.hookRefR1 : this.hookRefR2;
            string str = !this.useGun ? "hookRefR1" : "hookRefR2";
            this.bulletRight.transform.position = obj2.transform.position;
            Bullet component = this.bulletRight.GetComponent<Bullet>();
            float num = !single ? ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)) : 0f;
            Vector3 vector = (hit.point + ((Vector3) (base.transform.right * num))) - this.bulletRight.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch((Vector3) (vector * 5f), base.rigidbody.velocity, str, false, base.gameObject, true);
            }
            else
            {
                component.launch((Vector3) (vector * 3f), base.rigidbody.velocity, str, false, base.gameObject, false);
            }
            this.launchPointRight = Vector3.zero;
        }
    }

    private void leftArmAimTo(Vector3 target)
    {
        float y = target.x - this.upperarmL.transform.position.x;
        float num2 = target.y - this.upperarmL.transform.position.y;
        float x = target.z - this.upperarmL.transform.position.z;
        float num4 = Mathf.Sqrt((y * y) + (x * x));
        this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
        this.forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        this.upperarmL.rotation = Quaternion.Euler(0f, 90f + (Mathf.Atan2(y, x) * 57.29578f), -Mathf.Atan2(num2, num4) * 57.29578f);
    }

    public void markDie()
    {
        this.hasDied = true;
        this.state = HERO_STATE.Die;
    }

    [RPC]
    private void net3DMGSMOKE(bool ifON)
    {
        if (this.smoke_3dmg != null)
        {
            this.smoke_3dmg.enableEmission = ifON;
        }
    }

    [RPC]
    private void netContinueAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
	            disposable.Dispose();
            }
        }
        this.playAnimation(this.currentPlayingClipName());
    }

    [RPC]
    private void netCrossFade(string aniName, float time)
    {
        this.currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.CrossFade(aniName, time);
        }
    }

    [RPC]
    public void netDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
    {
        if ((base.photonView.isMine && this.titanForm) && (this.eren_titan != null))
        {
            this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        this.meatDie.Play();
        if (!this.useGun && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            this.leftbladetrail.Deactivate();
            this.rightbladetrail.Deactivate();
            this.leftbladetrail2.Deactivate();
            this.rightbladetrail2.Deactivate();
        }
        this.falseAttack();
        this.breakApart(v, isBite);
        if (base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().myRespawnTime = 0f;
        }
        this.hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int) PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
            if (viewID != -1)
            {
                PhotonView view = PhotonView.Find(viewID);
                if (view != null)
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(killByTitan, (string) view.owner.customProperties[PhotonPlayerProperty.name], false, (string) PhotonNetwork.player.customProperties[PhotonPlayerProperty.name], 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, ((int) view.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
                    view.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(!(titanName == string.Empty), titanName, false, (string) PhotonNetwork.player.customProperties[PhotonPlayerProperty.name], 0);
            }
        }
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
    }

    [RPC]
    private void netDie2(int viewID = -1, string titanName = "")
    {
        GameObject obj2;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
        }
        this.meatDie.Play();
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        if (base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().myRespawnTime = 0f;
        }
        this.falseAttack();
        this.hasDied = true;
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int) PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                PhotonView view = PhotonView.Find(viewID);
                if (view != null)
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(true, (string) view.owner.customProperties[PhotonPlayerProperty.name], false, (string) PhotonNetwork.player.customProperties[PhotonPlayerProperty.name], 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, ((int) view.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
                    view.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(true, titanName, false, (string) PhotonNetwork.player.customProperties[PhotonPlayerProperty.name], 0);
            }
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            obj2 = PhotonNetwork.Instantiate("hitMeat2", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
        }
        obj2.transform.position = base.transform.position;
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
    }

    [RPC]
    private void netGrabbed(int id, bool leftHand)
    {
        this.titanWhoGrabMeID = id;
        this.grabbed(PhotonView.Find(id).gameObject, leftHand);
    }

    [RPC]
    private void netlaughAttack()
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (((Vector3.Distance(obj2.transform.position, base.transform.position) < 50f) && (Vector3.Angle(obj2.transform.forward, base.transform.position - obj2.transform.position) < 90f)) && (obj2.GetComponent<TITAN>() != null))
            {
                obj2.GetComponent<TITAN>().beLaughAttacked();
            }
        }
    }

    [RPC]
    private void netPauseAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = 0f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
	            disposable.Dispose();
            }
        }
    }

    [RPC]
    private void netPlayAnimation(string aniName)
    {
        this.currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.Play(aniName);
        }
    }

    [RPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        this.currentAnimation = aniName;
        if (base.animation != null)
        {
            base.animation.Play(aniName);
            base.animation[aniName].normalizedTime = normalizedTime;
        }
    }

    [RPC]
    private void netSetIsGrabbedFalse()
    {
        this.state = HERO_STATE.Idle;
    }

    [RPC]
    private void netTauntAttack(float tauntTime, float distance = 100f)
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if ((Vector3.Distance(obj2.transform.position, base.transform.position) < distance) && (obj2.GetComponent<TITAN>() != null))
            {
                obj2.GetComponent<TITAN>().beTauntedBy(base.gameObject, tauntTime);
            }
        }
    }

    [RPC]
    private void netUngrabbed()
    {
        this.ungrabbed();
        this.netPlayAnimation(this.standAnimation);
        this.falseAttack();
    }

    private void OnDestroy()
    {
        if (this.myNetWorkName != null)
        {
            UnityEngine.Object.Destroy(this.myNetWorkName);
        }
        if (this.gunDummy != null)
        {
            UnityEngine.Object.Destroy(this.gunDummy);
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            this.releaseIfIHookSb();
        }
        if (GameObject.Find("MultiplayerManager") != null)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeHero(this);
        }
    }

    public void pauseAnimation()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = 0f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
	            disposable.Dispose();
            }
        }
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && base.photonView.isMine)
        {
            base.photonView.RPC("netPauseAnimation", PhotonTargets.Others, new object[0]);
        }
    }

    public void playAnimation(string aniName)
    {
        this.currentAnimation = aniName;
        base.animation.Play(aniName);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName };
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        this.currentAnimation = aniName;
        base.animation.Play(aniName);
        base.animation[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
        }
    }

    private void releaseIfIHookSb()
    {
        if (this.hookSomeOne && (this.hookTarget != null))
        {
            this.hookTarget.GetPhotonView().RPC("badGuyReleaseMe", this.hookTarget.GetPhotonView().owner, new object[0]);
            this.hookTarget = null;
            this.hookSomeOne = false;
        }
    }

    public void resetAnimationSpeed()
    {
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = 1f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
	            disposable.Dispose();
            }
        }
        this.customAnimationSpeed();
    }

    private void rightArmAimTo(Vector3 target)
    {
        float y = target.x - this.upperarmR.transform.position.x;
        float num2 = target.y - this.upperarmR.transform.position.y;
        float x = target.z - this.upperarmR.transform.position.z;
        float num4 = Mathf.Sqrt((y * y) + (x * x));
        this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        this.forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
        this.upperarmR.rotation = Quaternion.Euler(180f, 90f + (Mathf.Atan2(y, x) * 57.29578f), Mathf.Atan2(num2, num4) * 57.29578f);
    }

    [RPC]
    private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
    {
        this.hookBySomeOne = true;
        this.badGuy = PhotonView.Find(hooker).gameObject;
        if (Vector3.Distance(hookPosition, base.transform.position) < 15f)
        {
            this.launchForce = PhotonView.Find(hooker).gameObject.transform.position - base.transform.position;
            base.rigidbody.AddForce((Vector3) (-base.rigidbody.velocity * 0.9f), ForceMode.VelocityChange);
            float num = Mathf.Pow(this.launchForce.magnitude, 0.1f);
            if (this.grounded)
            {
                base.rigidbody.AddForce((Vector3) (Vector3.up * Mathf.Min((float) (this.launchForce.magnitude * 0.2f), (float) 10f)), ForceMode.Impulse);
            }
            base.rigidbody.AddForce((Vector3) ((this.launchForce * num) * 0.1f), ForceMode.Impulse);
            if (this.state != HERO_STATE.Grab)
            {
                this.dashTime = 1f;
                this.crossFade("dash", 0.05f);
                base.animation["dash"].time = 0.1f;
                this.state = HERO_STATE.AirDodge;
                this.falseAttack();
                this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
                Quaternion quaternion = Quaternion.Euler(0f, this.facingDirection, 0f);
                base.gameObject.transform.rotation = quaternion;
                base.rigidbody.rotation = quaternion;
                this.targetRotation = quaternion;
            }
        }
        else
        {
            this.hookBySomeOne = false;
            this.badGuy = null;
            PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner, new object[0]);
        }
    }

    private void salute()
    {
        this.state = HERO_STATE.Salute;
        this.crossFade("salute", 0.1f);
    }

    private void setHookedPplDirection()
    {
        this.almostSingleHook = false;
        if (this.isRightHandHooked && this.isLeftHandHooked)
        {
            if ((this.bulletLeft != null) && (this.bulletRight != null))
            {
                Vector3 normal = this.bulletLeft.transform.position - this.bulletRight.transform.position;
                if (normal.sqrMagnitude < 4f)
                {
                    Vector3 vector2 = ((Vector3) ((this.bulletLeft.transform.position + this.bulletRight.transform.position) * 0.5f)) - base.transform.position;
                    this.facingDirection = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
                    if (this.useGun && (this.state != HERO_STATE.Attack))
                    {
                        float current = -Mathf.Atan2(base.rigidbody.velocity.z, base.rigidbody.velocity.x) * 57.29578f;
                        float target = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                        float num3 = -Mathf.DeltaAngle(current, target);
                        this.facingDirection += num3;
                    }
                    this.almostSingleHook = true;
                }
                else
                {
                    Vector3 to = base.transform.position - this.bulletLeft.transform.position;
                    Vector3 vector4 = base.transform.position - this.bulletRight.transform.position;
                    Vector3 vector5 = (Vector3) ((this.bulletLeft.transform.position + this.bulletRight.transform.position) * 0.5f);
                    Vector3 from = base.transform.position - vector5;
                    if ((Vector3.Angle(from, to) < 30f) && (Vector3.Angle(from, vector4) < 30f))
                    {
                        this.almostSingleHook = true;
                        Vector3 vector7 = vector5 - base.transform.position;
                        this.facingDirection = Mathf.Atan2(vector7.x, vector7.z) * 57.29578f;
                    }
                    else
                    {
                        this.almostSingleHook = false;
                        Vector3 forward = base.transform.forward;
                        Vector3.OrthoNormalize(ref normal, ref forward);
                        this.facingDirection = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
                        float num4 = Mathf.Atan2(to.x, to.z) * 57.29578f;
                        if (Mathf.DeltaAngle(num4, this.facingDirection) > 0f)
                        {
                            this.facingDirection += 180f;
                        }
                    }
                }
            }
        }
        else
        {
            this.almostSingleHook = true;
            Vector3 zero = Vector3.zero;
            if (this.isRightHandHooked && (this.bulletRight != null))
            {
                zero = this.bulletRight.transform.position - base.transform.position;
            }
            else if (this.isLeftHandHooked && (this.bulletLeft != null))
            {
                zero = this.bulletLeft.transform.position - base.transform.position;
            }
            else
            {
                return;
            }
            this.facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
            if (this.state != HERO_STATE.Attack)
            {
                float num6 = -Mathf.Atan2(base.rigidbody.velocity.z, base.rigidbody.velocity.x) * 57.29578f;
                float num7 = -Mathf.Atan2(zero.z, zero.x) * 57.29578f;
                float num8 = -Mathf.DeltaAngle(num6, num7);
                if (this.useGun)
                {
                    this.facingDirection += num8;
                }
                else
                {
                    float num9 = 0f;
                    if ((this.isLeftHandHooked && (num8 < 0f)) || (this.isRightHandHooked && (num8 > 0f)))
                    {
                        num9 = -0.1f;
                    }
                    else
                    {
                        num9 = 0.1f;
                    }
                    this.facingDirection += num8 * num9;
                }
            }
        }
    }

    [RPC]
    private void setMyTeam(int val)
    {
        UnityEngine.MonoBehaviour.print("set team " + val);
        this.myTeam = val;
        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
    }

    public void setSkillHUDPosition()
    {
        this.skillCD = GameObject.Find("skill_cd_" + this.skillId);
        if (this.skillCD != null)
        {
            this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (this.useGun)
        {
            this.skillCD.transform.localPosition = (Vector3) (Vector3.up * 5000f);
        }
    }

    public void setStat()
    {
        this.skillCDLast = 1.5f;
        this.skillId = this.setup.myCostume.stat.skillId;
        if (this.skillId == "levi")
        {
            this.skillCDLast = 3.5f;
        }
        this.customAnimationSpeed();
        if (this.skillId == "armin")
        {
            this.skillCDLast = 5f;
        }
        if (this.skillId == "marco")
        {
            this.skillCDLast = 10f;
        }
        if (this.skillId == "jean")
        {
            this.skillCDLast = 0.001f;
        }
        if (this.skillId == "eren")
        {
            this.skillCDLast = 120f;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if ((LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || (LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.RACING)) || ((LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_CAPTURE) || (LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.TROST)))
                {
                    this.skillId = "petra";
                    this.skillCDLast = 1f;
                }
                else
                {
                    int num = 0;
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if ((((int) player.customProperties[PhotonPlayerProperty.isTitan]) == 1) && (((string) player.customProperties[PhotonPlayerProperty.character]).ToUpper() == "EREN"))
                        {
                            num++;
                        }
                    }
                    if (num > 1)
                    {
                        this.skillId = "petra";
                        this.skillCDLast = 1f;
                    }
                }
            }
        }
        if (this.skillId == "sasha")
        {
            this.skillCDLast = 20f;
        }
        if (this.skillId == "petra")
        {
            this.skillCDLast = 3.5f;
        }
        this.skillCDDuration = this.skillCDLast;
        this.speed = ((float) this.setup.myCostume.stat.SPD) / 10f;
        this.totalGas = this.currentGas = this.setup.myCostume.stat.GAS;
        this.totalBladeSta = this.currentBladeSta = this.setup.myCostume.stat.BLA;
        base.rigidbody.mass = 0.5f - ((this.setup.myCostume.stat.ACL - 100) * 0.001f);
        GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) + 5f, 0f);
        this.skillCD = GameObject.Find("skill_cd_" + this.skillId);
        this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = false;
            GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = false;
        }
        if (this.setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            this.standAnimation = "AHSS_stand_gun";
            this.useGun = true;
            this.gunDummy = new GameObject();
            this.gunDummy.name = "gunDummy";
            this.gunDummy.transform.position = base.transform.position;
            this.gunDummy.transform.rotation = base.transform.rotation;
            this.myGroup = GROUP.A;
            this.setTeam(2);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                GameObject.Find("bladeCL").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladeCR").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = true;
                GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = true;
                this.skillCD.transform.localPosition = (Vector3) (Vector3.up * 5000f);
            }
        }
        else if (this.setup.myCostume.sex == SEX.FEMALE)
        {
            this.standAnimation = "stand";
            this.setTeam(1);
        }
        else
        {
            this.standAnimation = "stand_levi";
            this.setTeam(1);
        }
    }

    public void setTeam(int team)
    {
        this.setMyTeam(team);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        {
            object[] parameters = new object[] { team };
            base.photonView.RPC("setMyTeam", PhotonTargets.OthersBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }

    public void shootFlare(int type)
    {
        bool flag = false;
        if ((type == 1) && (this.flare1CD == 0f))
        {
            this.flare1CD = this.flareTotalCD;
            flag = true;
        }
        if ((type == 2) && (this.flare2CD == 0f))
        {
            this.flare2CD = this.flareTotalCD;
            flag = true;
        }
        if ((type == 3) && (this.flare3CD == 0f))
        {
            this.flare3CD = this.flareTotalCD;
            flag = true;
        }
        if (flag)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/flareBullet" + type), base.transform.position, base.transform.rotation);
                obj2.GetComponent<FlareMovement>().dontShowHint();
                UnityEngine.Object.Destroy(obj2, 25f);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/flareBullet" + type, base.transform.position, base.transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
            }
        }
    }

    private void showAimUI()
    {
        Vector3 vector5;
        if (Screen.showCursor)
        {
            GameObject obj2 = GameObject.Find("cross1");
            GameObject obj3 = GameObject.Find("cross2");
            GameObject obj4 = GameObject.Find("crossL1");
            GameObject obj5 = GameObject.Find("crossL2");
            GameObject obj6 = GameObject.Find("crossR1");
            GameObject obj7 = GameObject.Find("crossR2");
            GameObject obj8 = GameObject.Find("LabelDistance");
            vector5 = (Vector3) (Vector3.up * 10000f);
            obj7.transform.localPosition = vector5;
            obj6.transform.localPosition = vector5;
            obj5.transform.localPosition = vector5;
            obj4.transform.localPosition = vector5;
            obj8.transform.localPosition = vector5;
            obj3.transform.localPosition = vector5;
            obj2.transform.localPosition = vector5;
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
            {
                RaycastHit hit2;
                GameObject obj9 = GameObject.Find("cross1");
                GameObject obj10 = GameObject.Find("cross2");
                obj9.transform.localPosition = Input.mousePosition;
                Transform transform = obj9.transform;
                transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                obj10.transform.localPosition = obj9.transform.localPosition;
                vector5 = hit.point - base.transform.position;
                float magnitude = vector5.magnitude;
                GameObject obj11 = GameObject.Find("LabelDistance");
                string str = (magnitude <= 1000f) ? ((int) magnitude).ToString() : "???";
                obj11.GetComponent<UILabel>().text = str;
                if (magnitude > 120f)
                {
                    Transform transform2 = obj9.transform;
                    transform2.localPosition += (Vector3) (Vector3.up * 10000f);
                    obj11.transform.localPosition = obj10.transform.localPosition;
                }
                else
                {
                    Transform transform3 = obj10.transform;
                    transform3.localPosition += (Vector3) (Vector3.up * 10000f);
                    obj11.transform.localPosition = obj9.transform.localPosition;
                }
                Transform transform4 = obj11.transform;
                transform4.localPosition -= new Vector3(0f, 15f, 0f);
                Vector3 vector = new Vector3(0f, 0.4f, 0f);
                vector -= (Vector3) (base.transform.right * 0.3f);
                Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
                vector2 += (Vector3) (base.transform.right * 0.3f);
                float num2 = (hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f);
                Vector3 vector3 = (hit.point - ((Vector3) (base.transform.right * num2))) - (base.transform.position + vector);
                Vector3 vector4 = (hit.point + ((Vector3) (base.transform.right * num2))) - (base.transform.position + vector2);
                vector3.Normalize();
                vector4.Normalize();
                vector3 = (Vector3) (vector3 * 1000000f);
                vector4 = (Vector3) (vector4 * 1000000f);
                if (Physics.Linecast(base.transform.position + vector, (base.transform.position + vector) + vector3, out hit2, mask3.value))
                {
                    GameObject obj12 = GameObject.Find("crossL1");
                    obj12.transform.localPosition = this.currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform5 = obj12.transform;
                    transform5.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj12.transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(obj12.transform.localPosition.y - (Input.mousePosition.y - (Screen.height * 0.5f)), obj12.transform.localPosition.x - (Input.mousePosition.x - (Screen.width * 0.5f))) * 57.29578f) + 180f);
                    GameObject obj13 = GameObject.Find("crossL2");
                    obj13.transform.localPosition = obj12.transform.localPosition;
                    obj13.transform.localRotation = obj12.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform6 = obj12.transform;
                        transform6.localPosition += (Vector3) (Vector3.up * 10000f);
                    }
                    else
                    {
                        Transform transform7 = obj13.transform;
                        transform7.localPosition += (Vector3) (Vector3.up * 10000f);
                    }
                }
                if (Physics.Linecast(base.transform.position + vector2, (base.transform.position + vector2) + vector4, out hit2, mask3.value))
                {
                    GameObject obj14 = GameObject.Find("crossR1");
                    obj14.transform.localPosition = this.currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform8 = obj14.transform;
                    transform8.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj14.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(obj14.transform.localPosition.y - (Input.mousePosition.y - (Screen.height * 0.5f)), obj14.transform.localPosition.x - (Input.mousePosition.x - (Screen.width * 0.5f))) * 57.29578f);
                    GameObject obj15 = GameObject.Find("crossR2");
                    obj15.transform.localPosition = obj14.transform.localPosition;
                    obj15.transform.localRotation = obj14.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform9 = obj14.transform;
                        transform9.localPosition += (Vector3) (Vector3.up * 10000f);
                    }
                    else
                    {
                        Transform transform10 = obj15.transform;
                        transform10.localPosition += (Vector3) (Vector3.up * 10000f);
                    }
                }
            }
        }
    }

    private void showFlareCD()
    {
        if (GameObject.Find("UIflare1") != null)
        {
            GameObject.Find("UIflare1").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare1CD) / this.flareTotalCD;
            GameObject.Find("UIflare2").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare2CD) / this.flareTotalCD;
            GameObject.Find("UIflare3").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare3CD) / this.flareTotalCD;
        }
    }

    private void showGas()
    {
        float num = this.currentGas / this.totalGas;
        float num2 = this.currentBladeSta / this.totalBladeSta;
        GameObject.Find("gasL1").GetComponent<UISprite>().fillAmount = this.currentGas / this.totalGas;
        GameObject.Find("gasR1").GetComponent<UISprite>().fillAmount = this.currentGas / this.totalGas;
        if (!this.useGun)
        {
            GameObject.Find("bladeCL").GetComponent<UISprite>().fillAmount = this.currentBladeSta / this.totalBladeSta;
            GameObject.Find("bladeCR").GetComponent<UISprite>().fillAmount = this.currentBladeSta / this.totalBladeSta;
            if (num <= 0f)
            {
                GameObject.Find("gasL").GetComponent<UISprite>().color = Color.red;
                GameObject.Find("gasR").GetComponent<UISprite>().color = Color.red;
            }
            else if (num < 0.3f)
            {
                GameObject.Find("gasL").GetComponent<UISprite>().color = Color.yellow;
                GameObject.Find("gasR").GetComponent<UISprite>().color = Color.yellow;
            }
            else
            {
                GameObject.Find("gasL").GetComponent<UISprite>().color = Color.white;
                GameObject.Find("gasR").GetComponent<UISprite>().color = Color.white;
            }
            if (num2 <= 0f)
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.red;
                GameObject.Find("blader1").GetComponent<UISprite>().color = Color.red;
            }
            else if (num2 < 0.3f)
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.yellow;
                GameObject.Find("blader1").GetComponent<UISprite>().color = Color.yellow;
            }
            else
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.white;
                GameObject.Find("blader1").GetComponent<UISprite>().color = Color.white;
            }
            if (this.currentBladeNum <= 4)
            {
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel5").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader5").GetComponent<UISprite>().enabled = true;
            }
            if (this.currentBladeNum <= 3)
            {
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel4").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader4").GetComponent<UISprite>().enabled = true;
            }
            if (this.currentBladeNum <= 2)
            {
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel3").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader3").GetComponent<UISprite>().enabled = true;
            }
            if (this.currentBladeNum <= 1)
            {
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel2").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader2").GetComponent<UISprite>().enabled = true;
            }
            if (this.currentBladeNum <= 0)
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
            }
            else
            {
                GameObject.Find("bladel1").GetComponent<UISprite>().enabled = true;
                GameObject.Find("blader1").GetComponent<UISprite>().enabled = true;
            }
        }
        else
        {
            if (this.leftGunHasBullet)
            {
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
            }
            else
            {
                GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            }
            if (this.rightGunHasBullet)
            {
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
            }
            else
            {
                GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            }
        }
    }

    [RPC]
    private void showHitDamage()
    {
        GameObject target = GameObject.Find("LabelScore");
        if (target != null)
        {
            this.speed = Mathf.Max(10f, this.speed);
            target.GetComponent<UILabel>().text = this.speed.ToString();
            target.transform.localScale = Vector3.zero;
            this.speed = (int) (this.speed * 0.1f);
            this.speed = Mathf.Clamp(this.speed, 40f, 150f);
            iTween.Stop(target);
            object[] args = new object[] { "x", this.speed, "y", this.speed, "z", this.speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
            iTween.ScaleTo(target, iTween.Hash(args));
            object[] objArray2 = new object[] { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f };
            iTween.ScaleTo(target, iTween.Hash(objArray2));
        }
    }

    private void showSkillCD()
    {
        if (this.skillCD != null)
        {
            this.skillCD.GetComponent<UISprite>().fillAmount = (this.skillCDLast - this.skillCDDuration) / this.skillCDLast;
        }
    }

    private void Start()
    {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addHero(this);
        if ((LevelInfo.getInfo(FengGameManagerMKII.level).horse && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && base.photonView.isMine)
        {
            this.myHorse = PhotonNetwork.Instantiate("horse", base.transform.position + ((Vector3) (Vector3.up * 5f)), base.transform.rotation, 0);
            this.myHorse.GetComponent<Horse>().myHero = base.gameObject;
        }
        bool flag = false;
        if ((Application.platform == RuntimePlatform.WindowsWebPlayer) || (Application.platform == RuntimePlatform.OSXWebPlayer))
        {
            if (Application.srcValue != "aog1.unity3d")
            {
                flag = true;
            }
            if (string.Compare(Application.absoluteURL, "http://fenglee.com/game/aog/aog1.unity3d", true) != 0)
            {
                flag = true;
            }
            if (flag)
            {
                Application.ExternalEval("top.location.href=\"http://fenglee.com/\";");
                UnityEngine.Object.Destroy(base.gameObject);
            }
            Application.ExternalEval("if(window != window.top) {document.location='http://fenglee.com/game/aog/playhere.html'}");
        }
        this.sparks = base.transform.Find("slideSparks").GetComponent<ParticleSystem>();
        this.smoke_3dmg = base.transform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        base.transform.localScale = new Vector3(this.myScale, this.myScale, this.myScale);
        this.facingDirection = base.transform.rotation.eulerAngles.y;
        this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
        this.smoke_3dmg.enableEmission = false;
        this.sparks.enableEmission = false;
        this.speedFXPS = this.speedFX1.GetComponent<ParticleSystem>();
        this.speedFXPS.enableEmission = false;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            GameObject obj2 = GameObject.Find("UI_IN_GAME");
            this.myNetWorkName = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            this.myNetWorkName.name = "LabelNameOverHead";
            this.myNetWorkName.transform.parent = obj2.GetComponent<UIReferArray>().panels[0].transform;
            this.myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
            this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;
            if (((int) base.photonView.owner.customProperties[PhotonPlayerProperty.team]) == 2)
            {
                this.myNetWorkName.GetComponent<UILabel>().text = "[FF0000]AHSS\n[FFFFFF]";
            }
            string str = (string) base.photonView.owner.customProperties[PhotonPlayerProperty.guildName];
            if (str != string.Empty)
            {
                UILabel component = this.myNetWorkName.GetComponent<UILabel>();
                string text = component.text;
                string[] textArray1 = new string[] { text, "[FFFF00]", str, "\n[FFFFFF]", (string) base.photonView.owner.customProperties[PhotonPlayerProperty.name] };
                component.text = string.Concat(textArray1);
            }
            else
            {
                UILabel local2 = this.myNetWorkName.GetComponent<UILabel>();
                local2.text = local2.text + ((string) base.photonView.owner.customProperties[PhotonPlayerProperty.name]);
            }
        }
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && !base.photonView.isMine)
        {
            base.gameObject.layer = LayerMask.NameToLayer("NetworkObject");
            if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
            {
                GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("flashlight"));
                obj3.transform.parent = base.transform;
                obj3.transform.position = base.transform.position + Vector3.up;
                obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            }
            this.setup.init();
            this.setup.myCostume = new HeroCostume();
            this.setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume(base.photonView.owner);
            this.setup.setCharacterComponent();
            UnityEngine.Object.Destroy(this.checkBoxLeft);
            UnityEngine.Object.Destroy(this.checkBoxRight);
            UnityEngine.Object.Destroy(this.leftbladetrail);
            UnityEngine.Object.Destroy(this.rightbladetrail);
            UnityEngine.Object.Destroy(this.leftbladetrail2);
            UnityEngine.Object.Destroy(this.rightbladetrail2);
        }
        else
        {
            this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        }
    }

    private void suicide()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            this.netDie((Vector3) (base.rigidbody.velocity * 50f), false, -1, string.Empty, true);
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().justSuicide = true;
        }
    }

    private void throwBlades()
    {
        Transform transform = this.setup.part_blade_l.transform;
        Transform transform2 = this.setup.part_blade_r.transform;
        GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
        GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
        obj2.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj3.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        Vector3 force = (base.transform.forward + ((Vector3) (base.transform.up * 2f))) - base.transform.right;
        obj2.rigidbody.AddForce(force, ForceMode.Impulse);
        Vector3 vector2 = (base.transform.forward + ((Vector3) (base.transform.up * 2f))) + base.transform.right;
        obj3.rigidbody.AddForce(vector2, ForceMode.Impulse);
        Vector3 torque = new Vector3((float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100));
        torque.Normalize();
        obj2.rigidbody.AddTorque(torque);
        torque = new Vector3((float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100));
        torque.Normalize();
        obj3.rigidbody.AddTorque(torque);
        this.setup.part_blade_l.SetActive(false);
        this.setup.part_blade_r.SetActive(false);
        this.currentBladeNum--;
        if (this.currentBladeNum == 0)
        {
            this.currentBladeSta = 0f;
        }
        if (this.state == HERO_STATE.Attack)
        {
            this.falseAttack();
        }
    }

    public void ungrabbed()
    {
        this.facingDirection = 0f;
        this.targetRotation = Quaternion.Euler(0f, 0f, 0f);
        base.transform.parent = null;
        base.GetComponent<CapsuleCollider>().isTrigger = false;
        this.state = HERO_STATE.Idle;
    }

    private void unmounted()
    {
        this.myHorse.GetComponent<Horse>().unmounted();
        this.isMounted = false;
    }

    public void update()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing)
        {
            if (this.invincible > 0f)
            {
                this.invincible -= Time.deltaTime;
            }
            if (!this.hasDied)
            {
                if (this.titanForm && (this.eren_titan != null))
                {
                    base.transform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
                    base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
                }
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
                {
                    if ((this.state == HERO_STATE.Grab) && !this.useGun)
                    {
                        if (this.skillId == "jean")
                        {
                            if (((this.state != HERO_STATE.Attack) && (this.inputManager.isInputDown[InputCode.attack0] || this.inputManager.isInputDown[InputCode.attack1])) && ((this.escapeTimes > 0) && !base.animation.IsPlaying("grabbed_jean")))
                            {
                                this.playAnimation("grabbed_jean");
                                base.animation["grabbed_jean"].time = 0f;
                                this.escapeTimes--;
                            }
                            if ((base.animation.IsPlaying("grabbed_jean") && (base.animation["grabbed_jean"].normalizedTime > 0.64f)) && (this.titanWhoGrabMe.GetComponent<TITAN>() != null))
                            {
                                this.ungrabbed();
                                base.rigidbody.velocity = (Vector3) (Vector3.up * 30f);
                                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                {
                                    this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                }
                                else
                                {
                                    base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                    }
                                    else
                                    {
                                        PhotonView.Find(this.titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient, new object[0]);
                                    }
                                }
                            }
                        }
                        else if (this.skillId == "eren")
                        {
                            this.showSkillCD();
                            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) || ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) && !IN_GAME_MAIN_CAMERA.isPausing))
                            {
                                this.calcSkillCD();
                                this.calcFlareCD();
                            }
                            if (this.inputManager.isInputDown[InputCode.attack1])
                            {
                                bool flag = false;
                                if ((this.skillCDDuration > 0f) || flag)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    this.skillCDDuration = this.skillCDLast;
                                    if ((this.skillId == "eren") && (this.titanWhoGrabMe.GetComponent<TITAN>() != null))
                                    {
                                        this.ungrabbed();
                                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                        {
                                            this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                        }
                                        else
                                        {
                                            base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
                                            if (PhotonNetwork.isMasterClient)
                                            {
                                                this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                            }
                                            else
                                            {
                                                PhotonView.Find(this.titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient, new object[0]);
                                            }
                                        }
                                        this.erenTransform();
                                    }
                                }
                            }
                        }
                    }
                    else if (!this.titanForm)
                    {
                        this.bufferUpdate();
                        if (!this.grounded && (this.state != HERO_STATE.AirDodge))
                        {
                            this.checkDashDoubleTap();
                            if (this.dashD)
                            {
                                this.dashD = false;
                                this.dash(0f, -1f);
                                return;
                            }
                            if (this.dashU)
                            {
                                this.dashU = false;
                                this.dash(0f, 1f);
                                return;
                            }
                            if (this.dashL)
                            {
                                this.dashL = false;
                                this.dash(-1f, 0f);
                                return;
                            }
                            if (this.dashR)
                            {
                                this.dashR = false;
                                this.dash(1f, 0f);
                                return;
                            }
                        }
                        if (this.grounded && ((this.state == HERO_STATE.Idle) || (this.state == HERO_STATE.Slide)))
                        {
                            if ((this.inputManager.isInputDown[InputCode.jump] && !base.animation.IsPlaying("jump")) && !base.animation.IsPlaying("horse_geton"))
                            {
                                this.idle();
                                this.crossFade("jump", 0.1f);
                                this.sparks.enableEmission = false;
                            }
                            if ((this.inputManager.isInputDown[InputCode.dodge] && !base.animation.IsPlaying("jump")) && !base.animation.IsPlaying("horse_geton"))
                            {
                                this.dodge(false);
                                return;
                            }
                        }
                        if (this.state == HERO_STATE.Idle)
                        {
                            if (this.inputManager.isInputDown[InputCode.flare1])
                            {
                                this.shootFlare(1);
                            }
                            if (this.inputManager.isInputDown[InputCode.flare2])
                            {
                                this.shootFlare(2);
                            }
                            if (this.inputManager.isInputDown[InputCode.flare3])
                            {
                                this.shootFlare(3);
                            }
                            if (this.inputManager.isInputDown[InputCode.restart])
                            {
                                this.suicide();
                            }
                            if (((this.myHorse != null) && this.isMounted) && this.inputManager.isInputDown[InputCode.dodge])
                            {
                                this.getOffHorse();
                            }
                            if ((base.animation.IsPlaying(this.standAnimation) || !this.grounded) && this.inputManager.isInputDown[InputCode.reload])
                            {
                                this.changeBlade();
                                return;
                            }
                            if (base.animation.IsPlaying(this.standAnimation) && this.inputManager.isInputDown[InputCode.salute])
                            {
                                this.salute();
                                return;
                            }
                            if ((!this.isMounted && (this.inputManager.isInputDown[InputCode.attack0] || this.inputManager.isInputDown[InputCode.attack1])) && !this.useGun)
                            {
                                bool flag2 = false;
                                if (this.inputManager.isInputDown[InputCode.attack1])
                                {
                                    if ((this.skillCDDuration > 0f) || flag2)
                                    {
                                        flag2 = true;
                                    }
                                    else
                                    {
                                        this.skillCDDuration = this.skillCDLast;
                                        if (this.skillId == "eren")
                                        {
                                            this.erenTransform();
                                            return;
                                        }
                                        if (this.skillId == "marco")
                                        {
                                            if (this.IsGrounded())
                                            {
                                                this.attackAnimation = (UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0";
                                                this.playAnimation(this.attackAnimation);
                                            }
                                            else
                                            {
                                                flag2 = true;
                                                this.skillCDDuration = 0f;
                                            }
                                        }
                                        else if (this.skillId == "armin")
                                        {
                                            if (this.IsGrounded())
                                            {
                                                this.attackAnimation = "special_armin";
                                                this.playAnimation("special_armin");
                                            }
                                            else
                                            {
                                                flag2 = true;
                                                this.skillCDDuration = 0f;
                                            }
                                        }
                                        else if (this.skillId == "sasha")
                                        {
                                            if (this.IsGrounded())
                                            {
                                                this.attackAnimation = "special_sasha";
                                                this.playAnimation("special_sasha");
                                                this.currentBuff = BUFF.SpeedUp;
                                                this.buffTime = 10f;
                                            }
                                            else
                                            {
                                                flag2 = true;
                                                this.skillCDDuration = 0f;
                                            }
                                        }
                                        else if (this.skillId == "mikasa")
                                        {
                                            this.attackAnimation = "attack3_1";
                                            this.playAnimation("attack3_1");
                                            base.rigidbody.velocity = (Vector3) (Vector3.up * 10f);
                                        }
                                        else if (this.skillId == "levi")
                                        {
                                            RaycastHit hit;
                                            this.attackAnimation = "attack5";
                                            this.playAnimation("attack5");
                                            Rigidbody rigidbody = base.rigidbody;
                                            rigidbody.velocity += (Vector3) (Vector3.up * 5f);
                                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                                            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask3 = mask2 | mask;
                                            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
                                            {
                                                if (this.bulletRight != null)
                                                {
                                                    this.bulletRight.GetComponent<Bullet>().disable();
                                                    this.releaseIfIHookSb();
                                                }
                                                this.dashDirection = hit.point - base.transform.position;
                                                this.launchRightRope(hit, true, 1);
                                                this.rope.Play();
                                            }
                                            this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
                                            this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                            this.attackLoop = 3;
                                        }
                                        else if (this.skillId == "petra")
                                        {
                                            RaycastHit hit2;
                                            this.attackAnimation = "special_petra";
                                            this.playAnimation("special_petra");
                                            Rigidbody rigidbody2 = base.rigidbody;
                                            rigidbody2.velocity += (Vector3) (Vector3.up * 5f);
                                            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask4 = ((int) 1) << LayerMask.NameToLayer("Ground");
                                            LayerMask mask5 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask6 = mask5 | mask4;
                                            if (Physics.Raycast(ray2, out hit2, 1E+07f, mask6.value))
                                            {
                                                if (this.bulletRight != null)
                                                {
                                                    this.bulletRight.GetComponent<Bullet>().disable();
                                                    this.releaseIfIHookSb();
                                                }
                                                if (this.bulletLeft != null)
                                                {
                                                    this.bulletLeft.GetComponent<Bullet>().disable();
                                                    this.releaseIfIHookSb();
                                                }
                                                this.dashDirection = hit2.point - base.transform.position;
                                                this.launchLeftRope(hit2, true, 0);
                                                this.launchRightRope(hit2, true, 0);
                                                this.rope.Play();
                                            }
                                            this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
                                            this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                            this.attackLoop = 3;
                                        }
                                        else
                                        {
                                            if (this.needLean)
                                            {
                                                if (this.leanLeft)
                                                {
                                                    this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                                }
                                                else
                                                {
                                                    this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                                }
                                            }
                                            else
                                            {
                                                this.attackAnimation = "attack1";
                                            }
                                            this.playAnimation(this.attackAnimation);
                                        }
                                    }
                                }
                                else if (this.inputManager.isInputDown[InputCode.attack0])
                                {
                                    if (this.needLean)
                                    {
                                        if (this.inputManager.isInput[InputCode.left])
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else if (this.inputManager.isInput[InputCode.right])
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                        else if (this.leanLeft)
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                    }
                                    else if (this.inputManager.isInput[InputCode.left])
                                    {
                                        this.attackAnimation = "attack2";
                                    }
                                    else if (this.inputManager.isInput[InputCode.right])
                                    {
                                        this.attackAnimation = "attack1";
                                    }
                                    else if (this.lastHook != null)
                                    {
                                        if (this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
                                        {
                                            this.attackAccordingToTarget(this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
                                        }
                                        else
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    else if ((this.bulletLeft != null) && (this.bulletLeft.transform.parent != null))
                                    {
                                        Transform a = this.bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (a != null)
                                        {
                                            this.attackAccordingToTarget(a);
                                        }
                                        else
                                        {
                                            this.attackAccordingToMouse();
                                        }
                                    }
                                    else if ((this.bulletRight != null) && (this.bulletRight.transform.parent != null))
                                    {
                                        Transform transform2 = this.bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (transform2 != null)
                                        {
                                            this.attackAccordingToTarget(transform2);
                                        }
                                        else
                                        {
                                            this.attackAccordingToMouse();
                                        }
                                    }
                                    else
                                    {
                                        GameObject obj2 = this.findNearestTitan();
                                        if (obj2 != null)
                                        {
                                            Transform transform3 = obj2.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                            if (transform3 != null)
                                            {
                                                this.attackAccordingToTarget(transform3);
                                            }
                                            else
                                            {
                                                this.attackAccordingToMouse();
                                            }
                                        }
                                        else
                                        {
                                            this.attackAccordingToMouse();
                                        }
                                    }
                                }
                                if (!flag2)
                                {
                                    this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                    this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                    if (this.grounded)
                                    {
                                        base.rigidbody.AddForce((Vector3) (base.gameObject.transform.forward * 200f));
                                    }
                                    this.playAnimation(this.attackAnimation);
                                    base.animation[this.attackAnimation].time = 0f;
                                    this.buttonAttackRelease = false;
                                    this.state = HERO_STATE.Attack;
                                    if ((this.grounded || (this.attackAnimation == "attack3_1")) || ((this.attackAnimation == "attack5") || (this.attackAnimation == "special_petra")))
                                    {
                                        this.attackReleased = true;
                                        this.buttonAttackRelease = true;
                                    }
                                    else
                                    {
                                        this.attackReleased = false;
                                    }
                                    this.sparks.enableEmission = false;
                                }
                            }
                            if (this.useGun)
                            {
                                if (this.inputManager.isInput[InputCode.attack1])
                                {
                                    this.leftArmAim = true;
                                    this.rightArmAim = true;
                                }
                                else if (this.inputManager.isInput[InputCode.attack0])
                                {
                                    if (this.leftGunHasBullet)
                                    {
                                        this.leftArmAim = true;
                                        this.rightArmAim = false;
                                    }
                                    else
                                    {
                                        this.leftArmAim = false;
                                        if (this.rightGunHasBullet)
                                        {
                                            this.rightArmAim = true;
                                        }
                                        else
                                        {
                                            this.rightArmAim = false;
                                        }
                                    }
                                }
                                else
                                {
                                    this.leftArmAim = false;
                                    this.rightArmAim = false;
                                }
                                if (this.leftArmAim || this.rightArmAim)
                                {
                                    RaycastHit hit3;
                                    Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    LayerMask mask7 = ((int) 1) << LayerMask.NameToLayer("Ground");
                                    LayerMask mask8 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                                    LayerMask mask9 = mask8 | mask7;
                                    if (Physics.Raycast(ray3, out hit3, 1E+07f, mask9.value))
                                    {
                                        this.gunTarget = hit3.point;
                                    }
                                }
                                bool flag3 = false;
                                bool flag4 = false;
                                bool flag5 = false;
                                if (this.inputManager.isInputUp[InputCode.attack1])
                                {
                                    if (this.leftGunHasBullet && this.rightGunHasBullet)
                                    {
                                        if (this.grounded)
                                        {
                                            this.attackAnimation = "AHSS_shoot_both";
                                        }
                                        else
                                        {
                                            this.attackAnimation = "AHSS_shoot_both_air";
                                        }
                                        flag3 = true;
                                    }
                                    else if (!this.leftGunHasBullet && !this.rightGunHasBullet)
                                    {
                                        flag4 = true;
                                    }
                                    else
                                    {
                                        flag5 = true;
                                    }
                                }
                                if (flag5 || this.inputManager.isInputUp[InputCode.attack0])
                                {
                                    if (this.grounded)
                                    {
                                        if (this.leftGunHasBullet && this.rightGunHasBullet)
                                        {
                                            if (this.isLeftHandHooked)
                                            {
                                                this.attackAnimation = "AHSS_shoot_r";
                                            }
                                            else
                                            {
                                                this.attackAnimation = "AHSS_shoot_l";
                                            }
                                        }
                                        else if (this.leftGunHasBullet)
                                        {
                                            this.attackAnimation = "AHSS_shoot_l";
                                        }
                                        else if (this.rightGunHasBullet)
                                        {
                                            this.attackAnimation = "AHSS_shoot_r";
                                        }
                                    }
                                    else if (this.leftGunHasBullet && this.rightGunHasBullet)
                                    {
                                        if (this.isLeftHandHooked)
                                        {
                                            this.attackAnimation = "AHSS_shoot_r_air";
                                        }
                                        else
                                        {
                                            this.attackAnimation = "AHSS_shoot_l_air";
                                        }
                                    }
                                    else if (this.leftGunHasBullet)
                                    {
                                        this.attackAnimation = "AHSS_shoot_l_air";
                                    }
                                    else if (this.rightGunHasBullet)
                                    {
                                        this.attackAnimation = "AHSS_shoot_r_air";
                                    }
                                    if (this.leftGunHasBullet || this.rightGunHasBullet)
                                    {
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        flag4 = true;
                                    }
                                }
                                if (flag3)
                                {
                                    this.state = HERO_STATE.Attack;
                                    this.crossFade(this.attackAnimation, 0.05f);
                                    this.gunDummy.transform.position = base.transform.position;
                                    this.gunDummy.transform.rotation = base.transform.rotation;
                                    this.gunDummy.transform.LookAt(this.gunTarget);
                                    this.attackReleased = false;
                                    this.facingDirection = this.gunDummy.transform.rotation.eulerAngles.y;
                                    this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                }
                                else if (flag4 && (this.grounded || (LevelInfo.getInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS)))
                                {
                                    this.changeBlade();
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.Attack)
                        {
                            if (!this.useGun)
                            {
                                if (!this.inputManager.isInput[InputCode.attack0])
                                {
                                    this.buttonAttackRelease = true;
                                }
                                if (!this.attackReleased)
                                {
                                    if (this.buttonAttackRelease)
                                    {
                                        this.continueAnimation();
                                        this.attackReleased = true;
                                    }
                                    else if (base.animation[this.attackAnimation].normalizedTime >= 0.32f)
                                    {
                                        this.pauseAnimation();
                                    }
                                }
                                if ((this.attackAnimation == "attack3_1") && (this.currentBladeSta > 0f))
                                {
                                    if (base.animation[this.attackAnimation].normalizedTime >= 0.8f)
                                    {
                                        if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.leftbladetrail2.Activate();
                                            this.rightbladetrail2.Activate();
                                            if (QualitySettings.GetQualityLevel() >= 2)
                                            {
                                                this.leftbladetrail.Activate();
                                                this.rightbladetrail.Activate();
                                            }
                                            base.rigidbody.velocity = (Vector3) (-Vector3.up * 30f);
                                        }
                                        if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.slash.Play();
                                        }
                                    }
                                    else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.leftbladetrail.StopSmoothly(0.1f);
                                        this.rightbladetrail.StopSmoothly(0.1f);
                                        this.leftbladetrail2.StopSmoothly(0.1f);
                                        this.rightbladetrail2.StopSmoothly(0.1f);
                                    }
                                }
                                else
                                {
                                    float num;
                                    float num2;
                                    if (this.currentBladeSta == 0f)
                                    {
                                        num = num2 = -1f;
                                    }
                                    else if (this.attackAnimation == "attack5")
                                    {
                                        num = 0.35f;
                                        num2 = 0.5f;
                                    }
                                    else if (this.attackAnimation == "special_petra")
                                    {
                                        num = 0.35f;
                                        num2 = 0.48f;
                                    }
                                    else if (this.attackAnimation == "special_armin")
                                    {
                                        num = 0.25f;
                                        num2 = 0.35f;
                                    }
                                    else if (this.attackAnimation == "attack4")
                                    {
                                        num = 0.6f;
                                        num2 = 0.9f;
                                    }
                                    else if (this.attackAnimation == "special_sasha")
                                    {
                                        num = num2 = -1f;
                                    }
                                    else
                                    {
                                        num = 0.5f;
                                        num2 = 0.85f;
                                    }
                                    if ((base.animation[this.attackAnimation].normalizedTime > num) && (base.animation[this.attackAnimation].normalizedTime < num2))
                                    {
                                        if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.slash.Play();
                                            this.leftbladetrail2.Activate();
                                            this.rightbladetrail2.Activate();
                                            if (QualitySettings.GetQualityLevel() >= 2)
                                            {
                                                this.leftbladetrail.Activate();
                                                this.rightbladetrail.Activate();
                                            }
                                        }
                                        if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        }
                                    }
                                    else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.leftbladetrail2.StopSmoothly(0.1f);
                                        this.rightbladetrail2.StopSmoothly(0.1f);
                                        if (QualitySettings.GetQualityLevel() >= 2)
                                        {
                                            this.leftbladetrail.StopSmoothly(0.1f);
                                            this.rightbladetrail.StopSmoothly(0.1f);
                                        }
                                    }
                                    if ((this.attackLoop > 0) && (base.animation[this.attackAnimation].normalizedTime > num2))
                                    {
                                        this.attackLoop--;
                                        this.playAnimationAt(this.attackAnimation, num);
                                    }
                                }
                                if (base.animation[this.attackAnimation].normalizedTime >= 1f)
                                {
                                    if ((this.attackAnimation == "special_marco_0") || (this.attackAnimation == "special_marco_1"))
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                object[] parameters = new object[] { 5f, 100f };
                                                base.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, parameters);
                                            }
                                            else
                                            {
                                                this.netTauntAttack(5f, 100f);
                                            }
                                        }
                                        else
                                        {
                                            this.netTauntAttack(5f, 100f);
                                        }
                                        this.falseAttack();
                                        this.idle();
                                    }
                                    else if (this.attackAnimation == "special_armin")
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                base.photonView.RPC("netlaughAttack", PhotonTargets.MasterClient, new object[0]);
                                            }
                                            else
                                            {
                                                this.netlaughAttack();
                                            }
                                        }
                                        else
                                        {
                                            foreach (GameObject obj3 in GameObject.FindGameObjectsWithTag("titan"))
                                            {
                                                if (((Vector3.Distance(obj3.transform.position, base.transform.position) < 50f) && (Vector3.Angle(obj3.transform.forward, base.transform.position - obj3.transform.position) < 90f)) && (obj3.GetComponent<TITAN>() != null))
                                                {
                                                    obj3.GetComponent<TITAN>().beLaughAttacked();
                                                }
                                            }
                                        }
                                        this.falseAttack();
                                        this.idle();
                                    }
                                    else if (this.attackAnimation == "attack3_1")
                                    {
                                        Rigidbody rigidbody3 = base.rigidbody;
                                        rigidbody3.velocity -= (Vector3) ((Vector3.up * Time.deltaTime) * 30f);
                                    }
                                    else
                                    {
                                        this.falseAttack();
                                        this.idle();
                                    }
                                }
                                if (base.animation.IsPlaying("attack3_2") && (base.animation["attack3_2"].normalizedTime >= 1f))
                                {
                                    this.falseAttack();
                                    this.idle();
                                }
                            }
                            else
                            {
                                base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.gunDummy.transform.rotation, Time.deltaTime * 30f);
                                UnityEngine.MonoBehaviour.print(this.attackAnimation);
                                if (!this.attackReleased && (base.animation[this.attackAnimation].normalizedTime > 0.167f))
                                {
                                    GameObject obj4;
                                    this.attackReleased = true;
                                    bool flag6 = false;
                                    if ((this.attackAnimation == "AHSS_shoot_both") || (this.attackAnimation == "AHSS_shoot_both_air"))
                                    {
                                        flag6 = true;
                                        this.leftGunHasBullet = false;
                                        this.rightGunHasBullet = false;
                                        base.rigidbody.AddForce((Vector3) (-base.transform.forward * 1000f), ForceMode.Acceleration);
                                    }
                                    else
                                    {
                                        if ((this.attackAnimation == "AHSS_shoot_l") || (this.attackAnimation == "AHSS_shoot_l_air"))
                                        {
                                            this.leftGunHasBullet = false;
                                        }
                                        else
                                        {
                                            this.rightGunHasBullet = false;
                                        }
                                        base.rigidbody.AddForce((Vector3) (-base.transform.forward * 600f), ForceMode.Acceleration);
                                    }
                                    base.rigidbody.AddForce((Vector3) (Vector3.up * 200f), ForceMode.Acceleration);
                                    string prefabName = "FX/shotGun";
                                    if (flag6)
                                    {
                                        prefabName = "FX/shotGun 1";
                                    }
                                    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
                                    {
                                        obj4 = PhotonNetwork.Instantiate(prefabName, (Vector3) ((base.transform.position + (base.transform.up * 0.8f)) - (base.transform.right * 0.1f)), base.transform.rotation, 0);
                                        if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                                        {
                                            obj4.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
                                        }
                                    }
                                    else
                                    {
                                        obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load(prefabName), (Vector3) ((base.transform.position + (base.transform.up * 0.8f)) - (base.transform.right * 0.1f)), base.transform.rotation);
                                    }
                                }
                                if (base.animation[this.attackAnimation].normalizedTime >= 1f)
                                {
                                    this.falseAttack();
                                    this.idle();
                                }
                                if (!base.animation.IsPlaying(this.attackAnimation))
                                {
                                    this.falseAttack();
                                    this.idle();
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.ChangeBlade)
                        {
                            if (this.useGun)
                            {
                                if (base.animation[this.reloadAnimation].normalizedTime > 0.22f)
                                {
                                    if (!this.leftGunHasBullet && this.setup.part_blade_l.activeSelf)
                                    {
                                        this.setup.part_blade_l.SetActive(false);
                                        Transform transform = this.setup.part_blade_l.transform;
                                        GameObject obj5 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
                                        obj5.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
                                        Vector3 force = ((Vector3) ((-base.transform.forward * 10f) + (base.transform.up * 5f))) - base.transform.right;
                                        obj5.rigidbody.AddForce(force, ForceMode.Impulse);
                                        Vector3 torque = new Vector3((float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100), (float) UnityEngine.Random.Range(-100, 100));
                                        obj5.rigidbody.AddTorque(torque, ForceMode.Acceleration);
                                    }
                                    if (!this.rightGunHasBullet && this.setup.part_blade_r.activeSelf)
                                    {
                                        this.setup.part_blade_r.SetActive(false);
                                        Transform transform5 = this.setup.part_blade_r.transform;
                                        GameObject obj6 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
                                        obj6.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
                                        Vector3 vector3 = ((Vector3) ((-base.transform.forward * 10f) + (base.transform.up * 5f))) + base.transform.right;
                                        obj6.rigidbody.AddForce(vector3, ForceMode.Impulse);
                                        Vector3 vector4 = new Vector3((float) UnityEngine.Random.Range(-300, 300), (float) UnityEngine.Random.Range(-300, 300), (float) UnityEngine.Random.Range(-300, 300));
                                        obj6.rigidbody.AddTorque(vector4, ForceMode.Acceleration);
                                    }
                                }
                                if ((base.animation[this.reloadAnimation].normalizedTime > 0.62f) && !this.throwedBlades)
                                {
                                    this.throwedBlades = true;
                                    if ((this.leftBulletLeft > 0) && !this.leftGunHasBullet)
                                    {
                                        this.leftBulletLeft--;
                                        this.setup.part_blade_l.SetActive(true);
                                        this.leftGunHasBullet = true;
                                    }
                                    if ((this.rightBulletLeft > 0) && !this.rightGunHasBullet)
                                    {
                                        this.setup.part_blade_r.SetActive(true);
                                        this.rightBulletLeft--;
                                        this.rightGunHasBullet = true;
                                    }
                                    this.updateRightMagUI();
                                    this.updateLeftMagUI();
                                }
                                if (base.animation[this.reloadAnimation].normalizedTime > 1f)
                                {
                                    this.idle();
                                }
                            }
                            else
                            {
                                if (!this.grounded)
                                {
                                    if ((base.animation[this.reloadAnimation].normalizedTime >= 0.2f) && !this.throwedBlades)
                                    {
                                        this.throwedBlades = true;
                                        if (this.setup.part_blade_l.activeSelf)
                                        {
                                            this.throwBlades();
                                        }
                                    }
                                    if ((base.animation[this.reloadAnimation].normalizedTime >= 0.56f) && (this.currentBladeNum > 0))
                                    {
                                        this.setup.part_blade_l.SetActive(true);
                                        this.setup.part_blade_r.SetActive(true);
                                        this.currentBladeSta = this.totalBladeSta;
                                    }
                                }
                                else
                                {
                                    if ((base.animation[this.reloadAnimation].normalizedTime >= 0.13f) && !this.throwedBlades)
                                    {
                                        this.throwedBlades = true;
                                        if (this.setup.part_blade_l.activeSelf)
                                        {
                                            this.throwBlades();
                                        }
                                    }
                                    if ((base.animation[this.reloadAnimation].normalizedTime >= 0.37f) && (this.currentBladeNum > 0))
                                    {
                                        this.setup.part_blade_l.SetActive(true);
                                        this.setup.part_blade_r.SetActive(true);
                                        this.currentBladeSta = this.totalBladeSta;
                                    }
                                }
                                if (base.animation[this.reloadAnimation].normalizedTime >= 1f)
                                {
                                    this.idle();
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.Salute)
                        {
                            if (base.animation["salute"].normalizedTime >= 1f)
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.GroundDodge)
                        {
                            if (base.animation.IsPlaying("dodge"))
                            {
                                if (!this.grounded && (base.animation["dodge"].normalizedTime > 0.6f))
                                {
                                    this.idle();
                                }
                                if (base.animation["dodge"].normalizedTime >= 1f)
                                {
                                    this.idle();
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.Land)
                        {
                            if (base.animation.IsPlaying("dash_land") && (base.animation["dash_land"].normalizedTime >= 1f))
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.FillGas)
                        {
                            if (base.animation.IsPlaying("supply") && (base.animation["supply"].normalizedTime >= 1f))
                            {
                                this.currentBladeSta = this.totalBladeSta;
                                this.currentBladeNum = this.totalBladeNum;
                                this.currentGas = this.totalGas;
                                if (!this.useGun)
                                {
                                    this.setup.part_blade_l.SetActive(true);
                                    this.setup.part_blade_r.SetActive(true);
                                }
                                else
                                {
                                    this.leftBulletLeft = this.rightBulletLeft = this.bulletMAX;
                                    this.leftGunHasBullet = this.rightGunHasBullet = true;
                                    this.setup.part_blade_l.SetActive(true);
                                    this.setup.part_blade_r.SetActive(true);
                                    this.updateRightMagUI();
                                    this.updateLeftMagUI();
                                }
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.Slide)
                        {
                            if (!this.grounded)
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.AirDodge)
                        {
                            if (this.dashTime > 0f)
                            {
                                this.dashTime -= Time.deltaTime;
                                if (this.currentSpeed > this.originVM)
                                {
                                    base.rigidbody.AddForce((Vector3) ((-base.rigidbody.velocity * Time.deltaTime) * 1.7f), ForceMode.VelocityChange);
                                }
                            }
                            else
                            {
                                this.dashTime = 0f;
                                this.idle();
                            }
                        }
                        if (((this.inputManager.isInput[InputCode.leftRope] && !base.animation.IsPlaying("attack3_1")) && (!base.animation.IsPlaying("attack5") && !base.animation.IsPlaying("special_petra"))) && ((this.state != HERO_STATE.ChangeBlade) && (this.state != HERO_STATE.Grab)))
                        {
                            if (this.bulletLeft != null)
                            {
                                this.QHold = true;
                            }
                            else
                            {
                                RaycastHit hit4;
                                Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask10 = ((int) 1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask11 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask12 = mask11 | mask10;
                                if (Physics.Raycast(ray4, out hit4, 10000f, mask12.value))
                                {
                                    this.launchLeftRope(hit4, true, 0);
                                    this.rope.Play();
                                }
                            }
                        }
                        else
                        {
                            this.QHold = false;
                        }
                        if (this.inputManager.isInput[InputCode.rightRope] && (((!base.animation.IsPlaying("attack3_1") && !base.animation.IsPlaying("attack5")) && !base.animation.IsPlaying("special_petra")) || (this.state == HERO_STATE.Idle)))
                        {
                            if (this.bulletRight != null)
                            {
                                this.EHold = true;
                            }
                            else
                            {
                                RaycastHit hit5;
                                Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask13 = ((int) 1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask14 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask15 = mask14 | mask13;
                                if (Physics.Raycast(ray5, out hit5, 10000f, mask15.value))
                                {
                                    this.launchRightRope(hit5, true, 0);
                                    this.rope.Play();
                                }
                            }
                        }
                        else
                        {
                            this.EHold = false;
                        }
                        if (this.inputManager.isInput[InputCode.bothRope] && (((!base.animation.IsPlaying("attack3_1") && !base.animation.IsPlaying("attack5")) && !base.animation.IsPlaying("special_petra")) || (this.state == HERO_STATE.Idle)))
                        {
                            this.QHold = true;
                            this.EHold = true;
                            if ((this.bulletLeft == null) && (this.bulletRight == null))
                            {
                                RaycastHit hit6;
                                Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask16 = ((int) 1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask17 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask18 = mask17 | mask16;
                                if (Physics.Raycast(ray6, out hit6, 1000000f, mask18.value))
                                {
                                    this.launchLeftRope(hit6, false, 0);
                                    this.launchRightRope(hit6, false, 0);
                                    this.rope.Play();
                                }
                            }
                        }
                        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) || ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) && !IN_GAME_MAIN_CAMERA.isPausing))
                        {
                            this.calcSkillCD();
                            this.calcFlareCD();
                        }
                        if (!this.useGun)
                        {
                            if (this.leftbladetrail.gameObject.GetActive())
                            {
                                this.leftbladetrail.update();
                                this.rightbladetrail.update();
                            }
                            if (this.leftbladetrail2.gameObject.GetActive())
                            {
                                this.leftbladetrail2.update();
                                this.rightbladetrail2.update();
                            }
                            if (this.leftbladetrail.gameObject.GetActive())
                            {
                                this.leftbladetrail.lateUpdate();
                                this.rightbladetrail.lateUpdate();
                            }
                            if (this.leftbladetrail2.gameObject.GetActive())
                            {
                                this.leftbladetrail2.lateUpdate();
                                this.rightbladetrail2.lateUpdate();
                            }
                        }
                        if (!IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            this.showSkillCD();
                            this.showFlareCD();
                            this.showGas();
                            this.showAimUI();
                        }
                    }
                }
            }
        }
    }

    private void updateLeftMagUI()
    {
        for (int i = 1; i <= this.bulletMAX; i++)
        {
            GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= this.leftBulletLeft; j++)
        {
            GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    private void updateRightMagUI()
    {
        for (int i = 1; i <= this.bulletMAX; i++)
        {
            GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= this.rightBulletLeft; j++)
        {
            GameObject.Find("bulletR" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    public void useBlade(int amount = 0)
    {
        if (amount == 0)
        {
            amount = 1;
        }
        amount *= 2;
        if (this.currentBladeSta > 0f)
        {
            this.currentBladeSta -= amount;
            if (this.currentBladeSta <= 0f)
            {
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
                {
                    this.leftbladetrail.Deactivate();
                    this.rightbladetrail.Deactivate();
                    this.leftbladetrail2.Deactivate();
                    this.rightbladetrail2.Deactivate();
                    this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                    this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                }
                this.currentBladeSta = 0f;
                this.throwBlades();
            }
        }
    }

    private void useGas(float amount = 0)
    {
        if (amount == 0f)
        {
            amount = this.useGasSpeed;
        }
        if (this.currentGas > 0f)
        {
            this.currentGas -= amount;
            if (this.currentGas < 0f)
            {
                this.currentGas = 0f;
            }
        }
    }

    [RPC]
    private void whoIsMyErenTitan(int id)
    {
        this.eren_titan = PhotonView.Find(id).gameObject;
        this.titanForm = true;
    }

    public bool isGrabbed
    {
        get
        {
            return (this.state == HERO_STATE.Grab);
        }
    }

    private HERO_STATE state
    {
        get
        {
            return this._state;
        }
        set
        {
            if ((this._state == HERO_STATE.AirDodge) || (this._state == HERO_STATE.GroundDodge))
            {
                this.dashTime = 0f;
            }
            UnityEngine.MonoBehaviour.print("state = " + value + " ");
            this._state = value;
        }
    }
}

