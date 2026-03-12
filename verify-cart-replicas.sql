-- ============================================
-- SQL Verification Script for InStock-Cart Replica Sync
-- ============================================

-- ============================================
-- PART 1: Quick Check - Count All Replicas
-- ============================================

SELECT 
    'Variant Replicas' as replica_type, 
    COUNT(*) as total_count 
FROM cart.in_stock_product_variant_replicas

UNION ALL

SELECT 
    'Inventory Replicas', 
    COUNT(*) 
FROM cart.in_stock_inventory_replicas

UNION ALL

SELECT 
    'Price Replicas', 
    COUNT(*) 
FROM cart.in_stock_price_replicas

UNION ALL

SELECT 
    'Price Detail Replicas', 
    COUNT(*) 
FROM cart.in_stock_product_price_detail_replicas;

-- Expected: See counts for each replica type

-- ============================================
-- PART 2: Verify Variant Replicas
-- ============================================

-- Check specific variants by SKU
SELECT 
    id,
    in_stock_product_id,
    sku,
    color,
    size,
    is_active,
    created_at,
    updated_at
FROM cart.in_stock_product_variant_replicas
WHERE sku IN ('MF-UCS-STD', 'MF-UCS-BLK', 'PKC-YEL-M', 'PKC-RED-M')
ORDER BY sku;

-- Check all variants
SELECT 
    sku,
    color,
    size,
    is_active,
    updated_at
FROM cart.in_stock_product_variant_replicas
ORDER BY created_at DESC
LIMIT 10;

-- ============================================
-- PART 3: Verify Inventory Replicas
-- ============================================

-- Check inventory with variant info
SELECT 
    i.id as inventory_id,
    v.sku,
    v.color,
    i.total_quantity,
    i.created_at,
    i.updated_at
FROM cart.in_stock_inventory_replicas i
JOIN cart.in_stock_product_variant_replicas v 
    ON i.in_stock_product_variant_id = v.id
ORDER BY i.updated_at DESC;

-- Check specific variant inventory
SELECT 
    v.sku,
    i.total_quantity,
    i.updated_at
FROM cart.in_stock_inventory_replicas i
JOIN cart.in_stock_product_variant_replicas v 
    ON i.in_stock_product_variant_id = v.id
WHERE v.sku = 'MF-UCS-STD';

-- ============================================
-- PART 4: Verify Price Replicas
-- ============================================

-- Check all prices
SELECT 
    id,
    name,
    effective_from,
    effective_to,
    priority,
    is_active,
    created_at,
    updated_at
FROM cart.in_stock_price_replicas
ORDER BY priority DESC, created_at DESC;

-- Check active prices for current date
SELECT 
    id,
    name,
    effective_from,
    effective_to,
    priority,
    is_active
FROM cart.in_stock_price_replicas
WHERE is_active = true
  AND NOW() BETWEEN effective_from AND effective_to
ORDER BY priority DESC;

-- ============================================
-- PART 5: Verify Price Detail Replicas
-- ============================================

-- Check all price details with variant and price info
SELECT 
    pd.id as price_detail_id,
    p.name as price_name,
    p.priority,
    v.sku as variant_sku,
    v.color,
    pd.unit_price,
    pd.is_active,
    pd.created_at,
    pd.updated_at
FROM cart.in_stock_product_price_detail_replicas pd
JOIN cart.in_stock_price_replicas p 
    ON pd.in_stock_price_id = p.id
JOIN cart.in_stock_product_variant_replicas v 
    ON pd.in_stock_product_variant_id = v.id
ORDER BY v.sku, p.priority DESC;

-- Find best price for a specific variant
SELECT 
    v.sku,
    v.color,
    p.name as price_name,
    pd.unit_price,
    p.priority,
    p.effective_from,
    p.effective_to
FROM cart.in_stock_product_price_detail_replicas pd
JOIN cart.in_stock_price_replicas p 
    ON pd.in_stock_price_id = p.id
JOIN cart.in_stock_product_variant_replicas v 
    ON pd.in_stock_product_variant_id = v.id
WHERE v.sku = 'MF-UCS-STD'
  AND pd.is_active = true
  AND p.is_active = true
  AND NOW() BETWEEN p.effective_from AND p.effective_to
ORDER BY p.priority DESC
LIMIT 1;

-- ============================================
-- PART 6: Complete Product View with All Data
-- ============================================

-- Get complete product variant information including inventory and prices
WITH variant_inventory AS (
    SELECT 
        v.id as variant_id,
        v.sku,
        v.color,
        v.size,
        v.is_active as variant_active,
        COALESCE(i.total_quantity, 0) as stock_quantity,
        i.updated_at as stock_updated
    FROM cart.in_stock_product_variant_replicas v
    LEFT JOIN cart.in_stock_inventory_replicas i 
        ON i.in_stock_product_variant_id = v.id
),
variant_prices AS (
    SELECT 
        pd.in_stock_product_variant_id as variant_id,
        json_agg(
            json_build_object(
                'price_name', p.name,
                'unit_price', pd.unit_price,
                'priority', p.priority,
                'effective_from', p.effective_from,
                'effective_to', p.effective_to,
                'is_active', pd.is_active AND p.is_active,
                'is_current', NOW() BETWEEN p.effective_from AND p.effective_to
            ) ORDER BY p.priority DESC
        ) as prices
    FROM cart.in_stock_product_price_detail_replicas pd
    JOIN cart.in_stock_price_replicas p 
        ON p.id = pd.in_stock_price_id
    GROUP BY pd.in_stock_product_variant_id
)
SELECT 
    vi.sku,
    vi.color,
    vi.size,
    vi.variant_active,
    vi.stock_quantity,
    vi.stock_updated,
    COALESCE(vp.prices, '[]'::json) as available_prices
FROM variant_inventory vi
LEFT JOIN variant_prices vp 
    ON vp.variant_id = vi.variant_id
ORDER BY vi.sku;

-- This gives you a complete view of each variant with inventory and all price options

-- ============================================
-- PART 7: Compare Source vs Replica
-- ============================================

-- Check if InStock and Cart data match (if you have access to both schemas)
-- Variant comparison
SELECT 
    'InStock Source' as source,
    COUNT(*) as count
FROM instock.instock_product_variants
UNION ALL
SELECT 
    'Cart Replica',
    COUNT(*)
FROM cart.in_stock_product_variant_replicas;

-- Inventory comparison
SELECT 
    'InStock Source' as source,
    COUNT(*) as count
FROM instock.instock_inventories
UNION ALL
SELECT 
    'Cart Replica',
    COUNT(*)
FROM cart.in_stock_inventory_replicas;

-- Price comparison
SELECT 
    'InStock Source' as source,
    COUNT(*) as count
FROM instock.instock_prices
UNION ALL
SELECT 
    'Cart Replica',
    COUNT(*)
FROM cart.in_stock_price_replicas;

-- Price Detail comparison
SELECT 
    'InStock Source' as source,
    COUNT(*) as count
FROM instock.instock_product_price_details
UNION ALL
SELECT 
    'Cart Replica',
    COUNT(*)
FROM cart.in_stock_product_price_detail_replicas;

-- ============================================
-- PART 8: Sync Verification - Recent Updates
-- ============================================

-- Check recently updated replicas (last 1 hour)
SELECT 
    'Variant' as replica_type,
    sku as identifier,
    updated_at
FROM cart.in_stock_product_variant_replicas
WHERE updated_at > NOW() - INTERVAL '1 hour'

UNION ALL

SELECT 
    'Inventory',
    CAST(in_stock_product_variant_id as TEXT),
    updated_at
FROM cart.in_stock_inventory_replicas
WHERE updated_at > NOW() - INTERVAL '1 hour'

UNION ALL

SELECT 
    'Price',
    name,
    updated_at
FROM cart.in_stock_price_replicas
WHERE updated_at > NOW() - INTERVAL '1 hour'

UNION ALL

SELECT 
    'Price Detail',
    CAST(id as TEXT),
    updated_at
FROM cart.in_stock_product_price_detail_replicas
WHERE updated_at > NOW() - INTERVAL '1 hour'

ORDER BY updated_at DESC;

-- ============================================
-- PART 9: Find Variants Ready for Cart
-- ============================================

-- Find variants that are:
-- 1. Active
-- 2. Have stock > 0
-- 3. Have at least one active price
SELECT 
    v.id as variant_id,
    v.sku,
    v.color,
    v.size,
    i.total_quantity,
    COUNT(DISTINCT pd.id) as active_price_count,
    MIN(pd.unit_price) as lowest_price,
    MAX(pd.unit_price) as highest_price,
    json_agg(
        json_build_object(
            'price_name', p.name,
            'unit_price', pd.unit_price,
            'priority', p.priority
        ) ORDER BY p.priority DESC
    ) as all_prices
FROM cart.in_stock_product_variant_replicas v
JOIN cart.in_stock_inventory_replicas i 
    ON i.in_stock_product_variant_id = v.id
JOIN cart.in_stock_product_price_detail_replicas pd 
    ON pd.in_stock_product_variant_id = v.id
JOIN cart.in_stock_price_replicas p 
    ON p.id = pd.in_stock_price_id
WHERE v.is_active = true
  AND i.total_quantity > 0
  AND pd.is_active = true
  AND p.is_active = true
  AND NOW() BETWEEN p.effective_from AND p.effective_to
GROUP BY v.id, v.sku, v.color, v.size, i.total_quantity
ORDER BY v.sku;

-- This query returns variants that can be added to cart right now

-- ============================================
-- PART 10: Cleanup Test Data (Optional)
-- ============================================

-- Delete all test replicas (if needed to reset)
-- WARNING: This will delete all replicas, only use in development!

/*
DELETE FROM cart.in_stock_product_price_detail_replicas;
DELETE FROM cart.in_stock_inventory_replicas;
DELETE FROM cart.in_stock_price_replicas;
DELETE FROM cart.in_stock_product_variant_replicas;

-- Verify deletion
SELECT 'Variant' as type, COUNT(*) FROM cart.in_stock_product_variant_replicas
UNION ALL
SELECT 'Inventory', COUNT(*) FROM cart.in_stock_inventory_replicas
UNION ALL
SELECT 'Price', COUNT(*) FROM cart.in_stock_price_replicas
UNION ALL
SELECT 'Price Detail', COUNT(*) FROM cart.in_stock_product_price_detail_replicas;
*/

-- ============================================
-- PART 11: Debug - Check Missing Replicas
-- ============================================

-- Find variants in InStock that don't have replicas in Cart
SELECT 
    v.id,
    v.sku,
    v.color,
    'Missing in Cart' as status
FROM instock.instock_product_variants v
LEFT JOIN cart.in_stock_product_variant_replicas r 
    ON r.id = v.id
WHERE r.id IS NULL;

-- Find inventories in InStock that don't have replicas in Cart
SELECT 
    i.id,
    v.sku,
    i.total_quantity,
    'Missing in Cart' as status
FROM instock.instock_inventories i
JOIN instock.instock_product_variants v 
    ON v.id = i.instock_product_variant_id
LEFT JOIN cart.in_stock_inventory_replicas r 
    ON r.id = i.id
WHERE r.id IS NULL;

-- ============================================
-- PART 12: Performance Check
-- ============================================

-- Check replica table sizes
SELECT 
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size,
    n_live_tup as row_count
FROM pg_stat_user_tables
WHERE schemaname = 'cart' 
  AND tablename LIKE '%replica%'
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;

-- ============================================
-- NOTES
-- ============================================

/*
TESTING TIPS:

1. Run API requests first from test-instock-cart-replica-sync.http
2. After each API call, run the corresponding SQL verification query
3. Check that created_at and updated_at timestamps are recent
4. Verify that updates are reflected in replicas immediately

EXPECTED BEHAVIOR:

? Create Variant ? Replica appears in cart.in_stock_product_variant_replicas
? Update Variant ? Replica updated_at changes, data syncs
? Create Inventory ? Replica appears in cart.in_stock_inventory_replicas
? Update Inventory ? Replica total_quantity changes
? Create Price ? Replica appears in cart.in_stock_price_replicas
? Update Price ? Replica data syncs
? Create Price Detail ? Replica appears in cart.in_stock_product_price_detail_replicas
? Update Price Detail ? Replica unit_price changes

DEBUGGING:

If replicas are not syncing:
1. Check application logs for "Publishing integration event"
2. Verify handlers are registered in DI
3. Check for exceptions in event handlers
4. Ensure IEventBus is properly configured
5. Verify database connections for both schemas

PERFORMANCE:

- Replicas should sync within milliseconds (in-memory event bus)
- Check pg_stat_user_tables for table statistics
- Monitor query performance on replica tables
- Add indexes if needed for frequently queried columns
*/
