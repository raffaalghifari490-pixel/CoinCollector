using UnityEngine;

public class Enemy : MonoBehaviour , IDamageable
{
    [Header("Pengaturan State Machine")]
    [SerializeField] private float jarakDeteksi = 6f;
    [SerializeField] private float jarakSerang = 1.5f;
    [SerializeField] private float jedaSerang = 1f;
    
    //state sekarang -- mulai dari IDLE
    private StateZombie state = StateZombie.IDLE;
    private float waktuSerangTerakhir;

    private int hp = 100;
    public float ms = 2f;

    protected Transform player;
    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {

        PeriksaTransisi();

        switch (state)
        {
            case StateZombie.IDLE: PerilakuIdle(); break;
            case StateZombie.PATROL: PerilakuPatrol(); break;
            case StateZombie.CHASE: PerilakuChase(); break;
            case StateZombie.ATTACK: PerilakuAttack(); break;
        }
    }

    public void Kejar()
    {
        if (player == null) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
        Debug.Log("Enemey Menyerang!");
    }
    
    void PerilakuIdle()
    {

    }

    void PerilakuPatrol()
    {
        Debug.Log(name + "; PATROL");
    }

    void PerilakuChase()
    {
        Kejar();
    }

    void PerilakuAttack()
    {
        if (Time.time >= waktuSerangTerakhir + jedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;
        }
    }

    public void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} terkena damage {jumlah}, sisa HP: {hp}");
        if (hp <= 0)
        {
            Mati();
        }
    }
    
    private void Mati()
    {
        Debug.Log($"{gameObject.name} mati");
        Destroy(gameObject);
    }
    public float JarakPlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakPlayer();

        if (jarak <= jarakSerang)
        state = StateZombie.ATTACK;
        else if(jarak <=jarakDeteksi)
        state = StateZombie.CHASE;
        else
        state = StateZombie.PATROL;
    }
}
