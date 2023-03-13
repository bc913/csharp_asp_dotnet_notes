using System;


/*
https://stackoverflow.com/questions/20004873/why-cant-the-operator-be-applied-to-a-struct-and-defaultstruct/20005260
https://montemagno.com/optimizing-c-struct-equality-with-iequatable/
http://www.albahari.com/valuevsreftypes.aspx
https://bettersolutions.com/csharp/interfaces/iequatable.htm
*/
namespace Bcan.Playground
{

    /*
    - For a value type, you should always implement IEquatable<T> and override Equals(Object) for better performance.
    - If you implement IEquatable<T>, you should also override the base class implementations of Equals(Object) and GetHashCode() so that their behavior is consistent with that of the Equals(T) method. If you do override Equals(Object), your overridden implementation is 
    also called in calls to the static Equals(System.Object, System.Object) method on your class. 
    - In addition, you should overload the op_Equality and op_Inequality operators. 
    This ensures that all tests for equality return consistent results.
    - If you implement IEquatable<T>, you should also implement IComparable<T> if instances of your type can be ordered or sorted. 
    - If your type implements IComparable<T>, you almost always also implement IEquatable<T>.
    */
    public struct Point : IEquatable<Point>
    {
        public int X;
        public int Y;

        public Point(int x, int y) => (X, Y) = (x, y);
        public override string ToString() => $"({X}, {Y})";

        #region System.Object Overrides
        // Always override Object.Equals because base impl uses reflection and boxes value types
        // so poorer performance.
        public override bool Equals(object obj)
        {
            return obj is Point mp && Equals(mp);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // return (X<< 2) ^ Y;
            return Tuple.Create(X, Y).GetHashCode();
        }
        #endregion

        #region IEquatable
        public bool Equals(Point other)
        {
            return other != null && X == other.X && Y == other.Y;
        }
        #endregion

        public static bool operator ==(Point lhs, Point rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Point lhs, Point rhs)
        {
            return !(lhs == rhs);
        }
    }
}