using UnityEngine;
using UnityEngine.InputSystem;

public class TPCamera : MonoBehaviour
{
    public Transform mFollowPoint;
    public Transform mFollowPointRef;

    public float mFollowDistance;
    public float mMinFollowDistance;
    public float mMaxFollowDistance;

    private float mVerticalDegree;
    public float mVerticalLimitUp;
    public float mVerticalLimitDown;

    private Vector3 mHorizontalVector;
    public float mMouseRotateSensitivity = 1.0f;
    public float followSpeed = 10.0f;
    private Vector3 mCurrentVel = Vector3.zero;
    public LayerMask mCheckLayer;

    public PlayerInput pInput;
    private InputAction moveAction;
    private InputAction lookAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = pInput.actions["Move"];
        lookAction = pInput.actions["Look"];

        mFollowPoint.position = mFollowPointRef.position;
        mFollowPoint.rotation = mFollowPointRef.rotation;
        //定位在followPt後一段距離
        transform.position = mFollowPoint.position - mFollowPoint.forward * mFollowDistance;
        Vector3 vDir = transform.position - mFollowPoint.position;
        //followPt到攝影機的水平向量
        mHorizontalVector = vDir;
        mHorizontalVector.y = 0;
        mHorizontalVector.Normalize();
    }

    //更新followPt位置
    public void UpdateFollowPt()
    {
        mFollowPoint.position = Vector3.Lerp(mFollowPoint.position, mFollowPointRef.position, 1.0f);
        Vector3 vDir = transform.position - mFollowPoint.position;
        vDir.y = 0.0f;
        vDir.Normalize();
        mHorizontalVector = Vector3.Lerp(mHorizontalVector, vDir, 10.0f * Time.deltaTime);
        mHorizontalVector.Normalize();
    }

    void LateUpdate()
    {
        //讀取滑鼠輸入
        Vector2 lookVector = lookAction.ReadValue<Vector2>();
        float fMX = lookVector.x * mMouseRotateSensitivity;
        float fMY = lookVector.y * mMouseRotateSensitivity;

        //水平旋轉，繞Y軸旋轉水平向量
        mHorizontalVector = Quaternion.AngleAxis(fMX, Vector3.up) * mHorizontalVector;
        //旋轉軸=水平向量與Y軸的垂直向量，準備給垂直旋轉使用
        Vector3 rotationAxis = Vector3.Cross(mHorizontalVector, Vector3.up);

        //限制垂直旋轉角度，-=fMY是因為滑鼠Y軸向下是正向
        mVerticalDegree -= fMY;
        if (mVerticalDegree < -mVerticalLimitUp)
        {
            mVerticalDegree = -mVerticalLimitUp;
        }
        else if (mVerticalDegree > mVerticalLimitDown)
        {
            mVerticalDegree = mVerticalLimitDown;
        }
        //最終旋轉後的向量=繞rotationAxis旋轉mVerticalDegree角度後的水平向量
        //旋轉順序是先水平旋轉再垂直旋轉，所以水平旋轉會影響rotationAxis，垂直旋轉會影響水平向量
        Vector3 vFinalDir = Quaternion.AngleAxis(mVerticalDegree, rotationAxis) * mHorizontalVector;
        vFinalDir.Normalize();

        //攝影機目標位置=followPt位置+旋轉後的水平向量*距離
        Vector3 vFinalPosition = mFollowPoint.position + vFinalDir * mFollowDistance;
        Vector3 vDir = mFollowPoint.position - vFinalPosition;
        vDir.Normalize();

        //檢查攝影機與followPt之間是否有障礙物，如果有，攝影機位置改為碰撞點+一小段距離
        RaycastHit rh;
        //vDir是從攝影機指向followPt的方向，所以要取反來當作射線方向
        Ray r  = new Ray(mFollowPoint.position, -vDir);

        if(Physics.SphereCast(r, 0.1f, out rh, mFollowDistance, mCheckLayer))
        {
            //碰撞點到followPt的距離-rh.distance就是攝影機應該移動的距離，攝影機位置=碰撞點+射線方向*0.1f
            vFinalPosition = mFollowPoint.position - vDir * (rh.distance - 0.1f);
        }

        transform.position = Vector3.Lerp(transform.position, vFinalPosition, followSpeed * Time.deltaTime);
        //transform.position = Vector3.SmoothDamp(transform.position, vFinalPosition, ref mCurrentVel, 0.05f, 10.0f);
        //transform.position = vFinalPosition;
        vDir = mFollowPoint.position - transform.position;
        transform.forward = vDir;
    }
}
