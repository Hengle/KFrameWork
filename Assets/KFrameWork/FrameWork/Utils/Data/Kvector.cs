﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KFrameWork
{
    public struct KInt : IEquatable<KInt>
    {
        public int X;
        private const float scale = 0.001f;
        private const int divscale = 1000;

        public static int isqrt(long x)
        {

            long remainder = x;

            long place = 1 << 30;//4 * 8 - 2

            while (place > remainder) { place /= 4; }

            long root = 0;
            while (place != 0)
            {
                if (remainder >= root + place)
                {
                    remainder -= root + place;
                    root += place * 2;
                }
                root /= 2;
                place /= 4;
            }

            if (x < 0)
            {
                root = -root;
            }

            return (int)root;
        }

        public bool Equals(KInt other)
        {
            return this.X != other.X;
        }

        public static implicit operator int(KInt i)
        {
            return i.X;
        }

        public static KInt operator +(KInt self, KInt other)
        {
            self.X += other.X;
            return self;
        }

        public static KInt operator +(KInt self, int other)
        {
            self.X +=other;
            return self;
        }

        public static KInt operator +(KInt self, long other)
        {
            self.X += (int)other;
            return self;
        }

        public static KInt operator -(KInt self, KInt other)
        {
            self.X -= other.X;
            return self;
        }

        public static KInt operator -(KInt self, int other)
        {
            self.X -= other;
            return self;
        }

        public static KInt operator -(KInt self, long other)
        {
            self.X -=(int) other;
            return self;
        }

        public static KInt operator *(KInt self, KInt other)
        {
            self.X *= other.X / divscale;
            return self;
        }

        public static KInt operator *(KInt self, int other)
        {
            self.X *= other;
            return self;
        }

        public static KInt operator *(KInt self, long other)
        {
            self.X *= (int)other;
            return self;
        }

        public static KInt operator /(KInt self, KInt other)
        {
            self.X /= other.X / divscale;
            return self;
        }

        public static KInt operator /(KInt self, int other)
        {
            self.X /= other / divscale;
            return self;
        }

        public static KInt operator /(KInt self, long other)
        {
            self.X /= (int)other / divscale;
            return self;
        }
    }

    [Serializable]
    /// <summary>
    /// 剔除vector2操作中的隐式转换，提倡在kint系列中多用kint，而不是vector，减少vector的使用,减少数据差异的可能性 ，set, up 会自动进行转换，IntX,Y,Z系列则直接覆盖
    /// </summary>
    public struct KInt2 : IEquatable<KInt2>
    {
        [SerializeField]
        private int _x;
        [SerializeField]
        private int _y;

        public const float scale = 0.001f;
        public const int divscale = 1000;

        private const int div2scale = divscale * divscale;

        public static readonly KInt2 zero = new KInt2(0, 0);
        public static readonly KInt2 one = new KInt2(1, 1);

        public float x
        {
            get
            {
                return _x * scale;
            }
        }

        public int IntX
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public float y
        {
            get
            {
                return _y * scale;
            }

        }

        public int IntY
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float magnitude
        {
            get
            {
                float xx = _x * scale * _x * scale;
                float yy = _y * scale * _y * scale;
                return Mathf.Sqrt(xx + yy );
            }
        }

        public int IntMagnitude
        {
            get
            {
                int xx = _x * _x;
                int yy = _y * _y;
#if UNITY_EDITOR
                if (xx + yy >= int.MaxValue || xx + yy  <= int.MinValue)
                {
                    LogMgr.LogError("Reach over");
                }
#endif
                int v = KInt.isqrt(xx + yy );
#if UNITY_EDITOR
                float kv = Mathf.Sqrt(xx + yy );
                if (Math.Abs(kv - v) > 1)
                {
                    LogMgr.LogError("error");
                    return (int)kv;
                }
#endif

                return v;
            }
        }

        public KInt2 normalized
        {
            get
            {
                int len = IntMagnitude;
                if (len == 0)
                {
                    return KInt2.zero;
                }

                KInt2 normlize = new KInt2();
                normlize.IntX = _x * divscale / len;
                normlize.IntY = _y * divscale / len;

                return normlize;
            }
        }


        public float sqrMagnitude
        {
            get
            {
                float xx = _x * scale * _x * scale;
                float yy = _y * scale * _y * scale;
                return xx + yy ;
            }
        }

        public int IntsqrMagnitude
        {
            get
            {
                int xx = _x * _x / (div2scale);
                int yy = _y * _y / (div2scale);
#if UNITY_EDITOR
                if (xx + yy >= int.MaxValue || xx + yy <= int.MinValue)
                {
                    LogMgr.LogError("Reach over");
                }
#endif
                return xx + yy;
            }
        }

        private KInt2(int kx, int ky)
        {
            this._x = kx * divscale;
            this._y = ky * divscale;
        }

        public KInt2(float kx, float ky)
        {
            this._x = (int)(kx * divscale);
            this._y = (int)(ky * divscale);
        }

        public KInt2(Vector3 pos)
        {
            this._x = (int)(pos.x * divscale);
            this._y = (int)(pos.y * divscale);
        }

        public void Copy(int px, int py)
        {
            this._x = px;
            this._y = py;
        }

        public void UpdateX(int dx)
        {
            this._x += dx * divscale;
        }

        public void UpdateX(short dx)
        {
            this._x += dx * divscale;
        }

        public void UpdateX(long dx)
        {
            this._x += (int)(dx * divscale);
        }

        public void UpdateX(float dx)
        {
            this._x += (int)(dx * divscale);
        }

        public void UpdateY(int dy)
        {
            this._y += dy * divscale;
        }

        public void UpdateY(short dy)
        {
            this._y += dy * divscale;
        }

        public void UpdateY(long dy)
        {
            this._y += (int)(dy * divscale);
        }

        public void UpdateY(float dy)
        {
            this._y += (int)(dy * divscale);
        }

        /// set
        /// 
        public void SetX(int dx)
        {
            this._x = dx * divscale;
        }

        public void SetX(short dx)
        {
            this._x = dx * divscale;
        }

        public void SetX(long dx)
        {
            this._x = (int)(dx * divscale);
        }

        public void SetX(float dx)
        {
            this._x = (int)(dx * divscale);
        }

        public void SetY(int dy)
        {
            this._y = dy * divscale;
        }

        public void SetY(short dy)
        {
            this._y = dy * divscale;
        }

        public void SetY(long dy)
        {
            this._y = (int)(dy * divscale);
        }

        public void SetY(float dy)
        {
            this._y = (int)(dy * divscale);
        }

        public static KInt2 Lerp(KInt2 a, KInt2 b, float f)
        {
            KInt2 data = new KInt2();
            data.IntX = (int)(a._x * (1f - f) + (b._x * f));
            data.IntY = (int)(a._y * (1f - f) + (b._y * f));
            return data;
        }

        public static KInt2 Lerp(KInt2 a, KInt2 b, int percent, int max)
        {
            KInt2 data = new KInt2();
            data.IntX = (a._x * (max - percent) / max + (b._x * percent) / max);
            data.IntY = (a._y * (max - percent) / max + (b._y * percent) / max);

            return data;
        }

        public static float Dot(KInt2 left, KInt2 right)
        {
            return left.x * right.x + left.y * right.y ;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", this._x * scale, this._y * scale);
        }


        public override int GetHashCode()
        {
            return this._x * 73856093 ^ this._y * 19349663 ;
        }

        public bool Equals(KInt2 other)
        {
            if (this._x != other._x) return false;
            if (this._y != other._y) return false;
            return true;
        }

        public override bool Equals(System.Object obj)
        {
            return base.Equals(obj);
        }

        public static implicit operator Vector2(KInt2 data)
        {
            return new Vector2(data._x * scale, data._y * scale);
        }

        public static explicit operator KInt2(Vector2 data)
        {
            return new KInt2(data);
        }

        public static KInt2 operator -(KInt2 vector)
        {
            KInt2 ki2  =new KInt2();
            ki2.IntX = -vector.IntX;
            ki2.IntY = -vector.IntY;
            return ki2;
        }

        #region add
        public static KInt2 operator +(KInt2 left, KInt2 right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x + right._x;
            value.IntY = left._y + right._y;
            return value;
        }

        public static KInt2 operator +(KInt2 left, Vector3 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x + right.x * divscale);
            value.IntY = (int)(left._y + right.y * divscale);
            return value;
        }

        public static KInt2 operator +(Vector2 right, KInt2 left)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x + right.x * divscale);
            value.IntY = (int)(left._y + right.y * divscale);
            return value;
        }

        public static KInt2 operator +(KInt2 left, int right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            return value;
        }

        public static KInt2 operator +(KInt2 left, short right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            return value;
        }

        public static KInt2 operator +(KInt2 left, float right)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x + r;
            value.IntY = left._y + r;
            return value;
        }

        public static KInt2 operator +(int right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            return value;
        }

        public static KInt2 operator +(short right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            return value;
        }

        public static KInt2 operator +(float right, KInt2 left)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x + r;
            value.IntY = left._y + r;
            return value;
        }


        #endregion
        #region reduce
        public static KInt2 operator -(KInt2 left, KInt2 right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x - right._x;
            value.IntY = left._y - right._y;
            return value;
        }

        public static KInt2 operator -(KInt2 left, Vector3 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x - right.x * divscale);
            value.IntY = (int)(left._y - right.y * divscale);
            return value;
        }

        public static KInt2 operator -(Vector2 left, KInt2 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left.x * divscale - right._x);
            value.IntY = (int)(left.y * divscale - right._y);
            return value;
        }

        public static KInt2 operator -(KInt2 left, int right)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            return value;
        }

        public static KInt2 operator -(KInt2 left, short right)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            return value;
        }

        public static KInt2 operator -(KInt2 left, float right)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            return value;
        }

        public static KInt2 operator -(int right, KInt2 left)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            return value;
        }

        public static KInt2 operator -(short right, KInt2 left)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            return value;
        }

        public static KInt2 operator -(float right, KInt2 left)
        {
            int r = (int)(right * divscale);
            KInt2 value = new KInt2();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            return value;
        }
        #endregion

        #region multi

        public static KInt2 operator *(KInt2 left, KInt2 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = left._x * right._x / divscale;
            value.IntY = left._y * right._y / divscale;
            return value;
        }

        public static KInt2 operator *(KInt2 left, Vector2 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x * right.x);
            value.IntY = (int)(left._y * right.y);

            return value;
        }

        public static KInt2 operator *(Vector2 left, KInt2 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left.x * right._x);
            value.IntY = (int)(left.y * right._y);
            return value;
        }

        public static KInt2 operator *(KInt2 left, int right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            return value;
        }

        public static KInt2 operator *(KInt2 left, short right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            return value;
        }

        public static KInt2 operator *(KInt2 left, float right)
        {
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x * right);
            value.IntY = (int)(left._y * right);
            return value;
        }

        public static KInt2 operator *(int right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            return value;
        }

        public static KInt2 operator *(short right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            return value;
        }

        public static KInt2 operator *(float right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x * right);
            value.IntY = (int)(left._y * right);
            return value;
        }

        #endregion

        #region div
        public static KInt2 operator /(KInt2 left, KInt2 right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x * divscale / right._x;
            value.IntY = left._y * divscale / right._y;
            return value;
        }

        public static KInt2 operator /(KInt2 left, Vector2 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x / right.x);
            value.IntY = (int)(left._y / right.y);
            return value;
        }

        public static KInt2 operator /(Vector2 left, KInt2 right)
        {
            //keep one scale
            KInt2 value = new KInt2();
            value.IntX = (int)(left.x * divscale / right._x);
            value.IntY = (int)(left.y * divscale / right._y);
            return value;
        }

        public static KInt2 operator /(KInt2 left, int right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            return value;
        }

        public static KInt2 operator /(KInt2 left, short right)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            return value;
        }

        public static KInt2 operator /(KInt2 left, float right)
        {
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x / right);
            value.IntY = (int)(left._y / right);
            return value;
        }

        public static KInt2 operator /(int right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            return value;
        }

        public static KInt2 operator /(short right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            return value;
        }

        public static KInt2 operator /(float right, KInt2 left)
        {
            KInt2 value = new KInt2();
            value.IntX = (int)(left._x / right);
            value.IntY = (int)(left._y / right);
            return value;
        }


        #endregion
        public static bool operator ==(KInt2 left, KInt2 right)
        {
            return left._x == right._x && left._y == right._y ;
        }

        public static bool operator !=(KInt2 left, KInt2 right)
        {
            return !(left._x == right._x && left._y == right._y );
        }

    }

    [Serializable]
    /// <summary>
    /// 剔除vector3操作中的隐式转换，提倡在kint系列中多用kint，而不是vector，减少vector的使用,减少数据差异的可能性 ，set, up 会自动进行转换，IntX,Y,Z系列则直接覆盖
    /// </summary>
    public struct KInt3:IEquatable<KInt3>
    {
        [SerializeField]
        private int _x;
        [SerializeField]
        private int _y;
        [SerializeField]
        private int _z;

        public const float scale = 0.001f;
        public const int divscale = 1000;

        private const int div2scale = divscale * divscale;

        public static readonly KInt3 zero = new KInt3(0, 0, 0);
        public static readonly KInt3 one = new KInt3(1, 1, 1);
        public static readonly KInt3 forward = new KInt3(0, 0, 1);
        public static readonly KInt3 up = new KInt3(0, 1, 0);
        public static readonly KInt3 right = new KInt3(1, 0, 0);

        public float x
        {
            get
            {
                return _x * scale;
            }
        }

        public int IntX
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public float y
        {
            get
            {
                return _y * scale;
            }

        }

        public int IntY
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float z
        {
            get
            {
                return _z * scale;
            }
        }

        public int IntZ
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }

        public float magnitude
        {
            get
            {
                float xx = _x * scale * _x * scale;
                float yy = _y * scale * _y * scale;
                float zz = _z * scale * _z * scale;
                return Mathf.Sqrt(xx+yy+zz);
            }
        }

        public int IntMagnitude
        {
            get
            {
                int xx = _x * _x;
                int yy = _y * _y;
                int zz = _z * _z;
#if UNITY_EDITOR
                if (xx + yy + zz >= int.MaxValue || xx+ yy + zz<= int.MinValue)
                {
                    LogMgr.LogError("Reach over");
                }
#endif
                int v = KInt.isqrt(xx + yy + zz);
#if UNITY_EDITOR
                float kv = Mathf.Sqrt(xx+yy+zz);
                if (Math.Abs(kv - v) > 1)
                {
                    LogMgr.LogError("error");
                    return (int)kv;
                }
#endif

                return v;
            }
        }

        public KInt3 normalized {
            get
            {
                int len = IntMagnitude;
                if (len == 0)
                {
                    return KInt3.zero;
                }

                KInt3 normlize = new KInt3();
                normlize.IntX = _x * divscale / len;
                normlize.IntY = _y * divscale / len;
                normlize.IntZ = _z * divscale / len;

                return normlize; 
            }
        }

        public long xzIntsqrMagnitude
        {
            get
            {
                long xx = _x  * _x / (div2scale) ;
                long zz = _z  * _z / (div2scale) ;
                return xx + zz;
            }
        }

        public float xzsqrMagnitude
        {
            get
            {
                float xx = _x * scale * _x * scale;
                float zz = _z * scale * _z * scale;
                return xx + zz;
            }
        }

        public float sqrMagnitude
        {
            get
            {
                float xx = _x * scale * _x * scale;
                float yy = _y * scale * _y * scale;
                float zz = _z * scale * _z * scale;
                return xx + yy + zz;
            }
        }

        public int IntsqrMagnitude
        {
            get
            {
                int xx = _x * _x /(div2scale);
                int yy = _y * _y / (div2scale);
                int zz = _z * _z / (div2scale);
#if UNITY_EDITOR
                if (xx + yy + zz >= int.MaxValue || xx + yy + zz <= int.MinValue)
                {
                    LogMgr.LogError("Reach over");
                }
#endif
                return xx + yy + zz;
            }
        }

        private KInt3(int kx, int ky, int kz)
        {
            this._x = kx * divscale;
            this._y = ky * divscale;
            this._z = kz * divscale;
        }

        public KInt3(float kx, float ky, float kz)
        {
            this._x =(int)( kx * divscale);
            this._y = (int)(ky * divscale);
            this._z = (int)(kz * divscale);
        }

        public KInt3(Vector3 pos)
        {
            this._x = (int)(pos.x * divscale);
            this._y = (int)(pos.y * divscale);
            this._z = (int)(pos.z * divscale);
        }

        public void Copy(int px,int py,int pz)
        {
            this._x = px;
            this._y = py;
            this._z = pz;
        }

        public void UpdateX(int dx)
        {
            this._x += dx * divscale;
        }

        public void UpdateX(short dx)
        {
            this._x += dx * divscale;
        }

        public void UpdateX(long dx)
        {
            this._x += (int)(dx * divscale);
        }

        public void UpdateX(float dx)
        {
            this._x += (int)(dx * divscale);
        }

        public void UpdateY(int dy)
        {
            this._y += dy * divscale;
        }

        public void UpdateY(short dy)
        {
            this._y += dy * divscale;
        }

        public void UpdateY(long dy)
        {
            this._y += (int)(dy * divscale);
        }

        public void UpdateY(float dy)
        {
            this._y += (int)(dy * divscale);
        }


        public void UpdateZ(int dz)
        {
            this._z += dz * divscale;
        }

        public void UpdateZ(short dz)
        {
            this._z += dz * divscale;
        }

        public void UpdateZ(long dz)
        {
            this._z += (int)(dz * divscale);
        }

        public void UpdateZ(float dz)
        {
            this._z += (int)(dz * divscale);
        }

        /// set
        /// 
        public void SetX(int dx)
        {
            this._x = dx * divscale;
        }

        public void SetX(short dx)
        {
            this._x = dx * divscale;
        }

        public void SetX(long dx)
        {
            this._x = (int)(dx * divscale);
        }

        public void SetX(float dx)
        {
            this._x = (int)(dx * divscale);
        }

        public void SetY(int dy)
        {
            this._y = dy * divscale;
        }

        public void SetY(short dy)
        {
            this._y = dy * divscale;
        }

        public void SetY(long dy)
        {
            this._y = (int)(dy * divscale);
        }

        public void SetY(float dy)
        {
            this._y = (int)(dy * divscale);
        }

        public void SetZ(int dz)
        {
            this._z = dz * divscale;
        }

        public void SetZ(short dz)
        {
            this._z = dz * divscale;
        }

        public void SetZ(long dz)
        {
            this._z = (int)(dz * divscale);
        }

        public void SetZ(float dz)
        {
            this._z = (int)(dz * divscale);
        }

        public static KInt3 Lerp(KInt3 a, KInt3 b, float f)
        {
            KInt3 data = new KInt3();
            data.IntX = (int)(a._x * (1f - f) + (b._x * f));
            data.IntY = (int)(a._y * (1f - f) + (b._y * f));
            data.IntZ = (int)(a._z * (1f - f) + (b._z * f));

            return data;
        }

        public static KInt3 Lerp(KInt3 a, KInt3 b, int percent,int max)
        {
            KInt3 data = new KInt3();
            data.IntX = (a._x * (max - percent) /max +  (b._x * percent) / max);
            data.IntY = (a._y * (max - percent) / max + (b._y * percent) / max);
            data.IntZ = (a._z * (max - percent) / max + (b._z * percent) / max);

            return data;
        }

        public static float Dot(KInt3 left,KInt3 right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1},{2}]", this._x * scale, this._y * scale, this._z * scale);
        }


        public override int GetHashCode()
        {
            return this._x * 73856093 ^ this._y * 19349663 ^ this._z * 83492791;
        }

        public bool Equals(KInt3 other)
        {
            if (this._x != other._x) return false;
            if (this._y != other._y) return false;
            if(this._z != other._z) return false;

            return true;
        }

        public override bool Equals(System.Object obj)
        {
            return base.Equals(obj);
        }

        public static implicit operator Vector3(KInt3 data)
        {
            return new Vector3(data._x * scale,data._y * scale,data._z *scale);
        }

        public static explicit operator KInt3(Vector3 data)
        {
            return new KInt3(data);
        }

        public static KInt3 operator -(KInt3 vector)
        {
            KInt3 ki3 = new KInt3();
            ki3.IntX = -vector.IntX;
            ki3.IntY = -vector.IntY;
            ki3.IntZ = -vector.IntZ;
            return ki3;
        }

        #region add
        public static KInt3 operator +(KInt3 left, KInt3 right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x + right._x;
            value.IntY = left._y + right._y;
            value.IntZ = left._z + right._z;
            return value;
        }

        public static KInt3 operator +(KInt3 left, Vector3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x + right.x * divscale);
            value.IntY = (int)(left._y + right.y * divscale);
            value.IntZ = (int)(left._z + right.z * divscale);
            return value;
        }

        public static KInt3 operator +(Vector3 right, KInt3 left)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x + right.x * divscale);
            value.IntY = (int)(left._y + right.y * divscale);
            value.IntZ = (int)(left._z + right.z * divscale);
            return value;
        }

        public static KInt3 operator +(KInt3 left, int right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x + right *divscale;
            value.IntY = left._y + right * divscale;
            value.IntZ = left._z + right * divscale;
            return value;
        }

        public static KInt3 operator +(KInt3 left, short right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            value.IntZ = left._z + right * divscale;
            return value;
        }

        public static KInt3 operator +(KInt3 left, float right)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x + r;
            value.IntY = left._y + r;
            value.IntZ = left._z + r;
            return value;
        }

        public static KInt3 operator +(int right ,KInt3 left )
        {
            KInt3 value = new KInt3();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            value.IntZ = left._z + right * divscale;
            return value;
        }

        public static KInt3 operator +( short right,KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x + right * divscale;
            value.IntY = left._y + right * divscale;
            value.IntZ = left._z + right * divscale;
            return value;
        }

        public static KInt3 operator +( float right, KInt3 left)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x + r;
            value.IntY = left._y + r;
            value.IntZ = left._z + r;
            return value;
        }


        #endregion
        #region reduce
        public static KInt3 operator -(KInt3 left, KInt3 right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x - right._x;
            value.IntY = left._y - right._y;
            value.IntZ = left._z - right._z;
            return value;
        }

        public static KInt3 operator -(KInt3 left, Vector3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x - right.x * divscale);
            value.IntY = (int)(left._y - right.y * divscale);
            value.IntZ = (int)(left._z - right.z * divscale);
            return value;
        }

        public static KInt3 operator -(Vector3 left, KInt3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left.x * divscale - right._x );
            value.IntY = (int)(left.y * divscale - right._y );
            value.IntZ = (int)(left.z * divscale - right._z );
            return value;
        }

        public static KInt3 operator -(KInt3 left, int right)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            value.IntZ = left._z - r;
            return value;
        }

        public static KInt3 operator -(KInt3 left, short right)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            value.IntZ = left._z - r;
            return value;
        }

        public static KInt3 operator -(KInt3 left, float right)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            value.IntZ = left._z - r;
            return value;
        }

        public static KInt3 operator -( int right, KInt3 left)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            value.IntZ = left._z - r;
            return value;
        }

        public static KInt3 operator -( short right, KInt3 left)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            value.IntZ = left._z - r;
            return value;
        }

        public static KInt3 operator -(float right, KInt3 left)
        {
            int r = (int)(right * divscale);
            KInt3 value = new KInt3();
            value.IntX = left._x - r;
            value.IntY = left._y - r;
            value.IntZ = left._z - r;
            return value;
        }
        #endregion

        #region multi

        public static KInt3 operator *(KInt3 left, KInt3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = left._x * right._x / divscale;
            value.IntY = left._y * right._y / divscale;
            value.IntZ = left._z * right._z / divscale;
            return value;
        }

        public static KInt3 operator *(KInt3 left, Vector3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX =(int) (left._x * right.x);
            value.IntY = (int)(left._y * right.y );
            value.IntZ = (int) (left._z * right.z) ;
            return value;
        }

        public static KInt3 operator *( Vector3 left, KInt3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left.x * right._x);
            value.IntY = (int)(left.y * right._y);
            value.IntZ = (int)(left.z * right._z);
            return value;
        }

        public static KInt3 operator *(KInt3 left, int right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            value.IntZ = left._z * right;
            return value;
        }

        public static KInt3 operator *(KInt3 left, short right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            value.IntZ = left._z * right;
            return value;
        }

        public static KInt3 operator *(KInt3 left, float right)
        {
            KInt3 value = new KInt3();
            value.IntX = (int) (left._x * right);
            value.IntY = (int)(left._y * right);
            value.IntZ = (int)(left._z * right);
            return value;
        }

        public static KInt3 operator *(int right, KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            value.IntZ = left._z * right;
            return value;
        }

        public static KInt3 operator *( short right, KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x * right;
            value.IntY = left._y * right;
            value.IntZ = left._z * right;
            return value;
        }

        public static KInt3 operator *( float right, KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x * right);
            value.IntY = (int)(left._y * right);
            value.IntZ = (int)(left._z * right);
            return value;
        }

        #endregion

        #region div
        public static KInt3 operator /(KInt3 left, KInt3 right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x * divscale / right._x;
            value.IntY = left._y * divscale / right._y;
            value.IntZ = left._z * divscale / right._z;
            return value;
        }

        public static KInt3 operator /(KInt3 left, Vector3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x / right.x);
            value.IntY = (int)(left._y / right.y);
            value.IntZ = (int)(left._z / right.z);
            return value;
        }

        public static KInt3 operator /(Vector3 left, KInt3 right)
        {
            //keep one scale
            KInt3 value = new KInt3();
            value.IntX = (int)(left.x * divscale / right._x);
            value.IntY = (int)(left.y * divscale / right._y);
            value.IntZ = (int)(left.z * divscale / right._z);
            return value;
        }

        public static KInt3 operator /(KInt3 left, int right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            value.IntZ = left._z / right;
            return value;
        }

        public static KInt3 operator /(KInt3 left, short right)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            value.IntZ = left._z / right;
            return value;
        }

        public static KInt3 operator /(KInt3 left, float right)
        {
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x / right);
            value.IntY = (int)(left._y / right);
            value.IntZ = (int)(left._z / right);
            return value;
        }

        public static KInt3 operator /( int right, KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            value.IntZ = left._z / right;
            return value;
        }

        public static KInt3 operator /( short right, KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = left._x / right;
            value.IntY = left._y / right;
            value.IntZ = left._z / right;
            return value;
        }

        public static KInt3 operator /( float right, KInt3 left)
        {
            KInt3 value = new KInt3();
            value.IntX = (int)(left._x / right);
            value.IntY = (int)(left._y / right);
            value.IntZ = (int)(left._z / right);
            return value;
        }


        #endregion
        public static bool operator ==(KInt3 left,KInt3 right)
        {
            return left._x == right._x && left._y == right._y && left._z == right._z;
        }

        public static bool operator !=(KInt3 left, KInt3 right)
        {
            return !(left._x == right._x && left._y == right._y && left._z == right._z);
        }

    }
}

