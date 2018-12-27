namespace Inheritance.Geometry
{
    public static class BodyExtensions
    {
        public static Dimensions GetDimensions(this Body body)
        {
            var visitor = new DimensionsVisitor();

            // Этот трюк с dynamic нужен, чтобы код компилировался, 
            // пока вы выполняете первую задачу и ещё не создали метод Body.Accept.
            // В реальном коде он не нужен, а можно просто вызывать body.Accept(...)
            dynamic dynamicBody = body; 
            dynamicBody.Accept(visitor);
            return visitor.Dimensions;
        }

        public static double GetSurfaceArea(this Body body)
        {
            var visitor = new SurfaceAreaVisitor();
            
            // см описание этого трюка в GetDimensions
            dynamic dynamicBody = body;
            dynamicBody.Accept(visitor);
            return visitor.SurfaceArea;
        }
    }
}